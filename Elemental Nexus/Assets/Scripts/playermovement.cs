using UnityEngine;

public class playermovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;

    private void Awake(){
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update(){
        float horizontalInput = Input.GetAxis("Horizontal");
    
        if(horizontalInput > 0.01f){
            transform.localScale = Vector3.one;
        }else if(horizontalInput < -0.01f){
            transform.localScale = new Vector3(-1, 1, 1);
        }

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        if(wallJumpCooldown > 0.5f){
            if(Input.GetKey(KeyCode.Space)){
                Jump();
            }

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(onWall() && !isGrounded() && body.velocity.y == 0){
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }else{
                body.gravityScale = 1;
            }
        }
        else{
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump(){
        if(isGrounded()){
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded()){
            wallJumpCooldown = 0;
            
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            print("Walljump triggered");
        }
    }
    
    private bool isGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
