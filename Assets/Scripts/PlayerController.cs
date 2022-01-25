using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    public float RunningSpeed;
    private float CurrentRunningSpeeds;
    public Transform transform_;
    public GameObject Stairs_on_bag;
    public GameObject Used_stair;
    public List<Stairs_on_bag> AllStairs;
    public bool Isground=true;
    public Animator animator;
    public int game_counter = 0;
    public Text tap_to_start;
    public Text game_over;
    public Text victory;
    public Text next;
    bool Can_dead = false;
    float finishZ=0;
    float finishX =0;
    int ActiveSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        Current = this;
        transform_=this.GetComponent<Transform>();
        CurrentRunningSpeeds = RunningSpeed;
        animator = GetComponent<Animator>();
        

    }

    // Update is called once per frame
    void Update()
    {
        ActiveSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (game_counter==1)
        {
            tap_to_start.enabled = false;
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + CurrentRunningSpeeds * Time.deltaTime);
            transform.position = newPosition;
        }
        else if (game_counter == 2)
        {
            game_over.enabled = true;
            animator.SetTrigger("dead");
            
            game_counter = 10;
        }
        else if (game_counter == 3)
        {
            victory.enabled = true;
            next.enabled = true; 
            game_counter = 4;
        }

        else if (transform.position.y > 0.3f)
        {
            Isground = false;
        }
        else if (transform.position.y < 1.4f)
        {
            Isground = true;
        }
    }
    private void FixedUpdate()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            AllStairs[AllStairs.Count - 1].UseStairFromBag();

            if (game_counter==0)
            {
                game_counter = 1;
            }
            else if (game_counter == 2)
            {
                SceneManager.LoadScene(0);
            }
            else if (next.enabled == true)
            {
                SceneManager.LoadScene((ActiveSceneIndex + 1)%3);
            }
            else if (game_counter == 10)
            {
                Restart();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (game_counter == 0)
            {
                game_counter = 1;
            }
            else if ( game_counter != 4 && game_counter != 3 && game_counter != 10)
            {
                AllStairs[AllStairs.Count - 1].UseStairFromBag();
            }
            else if (game_counter == 10)
            {
                Restart();
            }
            if (next.enabled == true)
            {
                SceneManager.LoadScene((ActiveSceneIndex + 1) % 3);
            }
        }
        else if (!Isground)
        {
            animator.ResetTrigger("run");
            animator.SetTrigger("fall");
        }
        else if (Isground)
        {
            animator.ResetTrigger("fall");
            animator.SetTrigger("run");
        }
    }
    void Restart()
    {
    
        SceneManager.LoadScene(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "stairs")
        {
            Can_dead = true;
            Destroy(other.gameObject);
            CreateStair();
        }
        else if (other.gameObject.tag == "enemy")
        {
            LoseStair();
            if (AllStairs.Count < 1 && Can_dead)
            {
                game_counter = 2;
            }
        }
        else if (other.gameObject.tag == "finish_line")
        {
            finishZ = 0.1f;
            finishX = 0.7f;
            game_counter = 5;
        }
        else if (other.gameObject.tag == "finish")
        {
            animator.SetTrigger("finish");
            game_counter = 3;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "finish")
        {
            animator.SetTrigger("finish");
            game_counter = 3;
        }
    }
    public void CreateStair()
    {
        Stairs_on_bag createdStair = Instantiate(Stairs_on_bag, transform).GetComponent<Stairs_on_bag>();
        AllStairs.Add(createdStair);
        createdStair.AddNewStair();
    }

    public void LoseStair()
    {
  
         transform.position += new Vector3(0, 4, -4);
        if (AllStairs.Count > 15)
        {
            for (int i = 1; i < 15; i++)
            {
                stairCrash(AllStairs[AllStairs.Count - 1]);
            }
        }
        else
        {
            Debug.Log(AllStairs.Count);
            for (int i = 0; i < AllStairs.Count; i++)
            {
                stairCrash(AllStairs[AllStairs.Count - 1]);
            }
        }
    }
    public void UseStair(Stairs_on_bag UsedStair)
    {
        this.transform.position += new Vector3(0, 0.3f, 0.2f+finishZ );
        GameObject Used_stair_clone = Instantiate(Used_stair);

        Used_stair_clone.transform.position = this.transform.position + new Vector3(0,0,0.25f);
        Used_stair_clone.GetComponent<Rigidbody>().useGravity = true;
        Used_stair_clone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Destroy(UsedStair.gameObject);
        AllStairs.Remove(AllStairs[AllStairs.Count - 1]);
        Destroy(Used_stair_clone.gameObject,1);
    }
    public void stairCrash(Stairs_on_bag CrashedStair)
    {
        Vector3 temp = CrashedStair.transform.position;
        CrashedStair.transform.parent = null;
        CrashedStair.transform.position = temp;
        CrashedStair.GetComponent<Rigidbody>().useGravity = true;
        CrashedStair.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        AllStairs.Remove(AllStairs[AllStairs.Count - 1]);

        Destroy(CrashedStair.gameObject, 2);
    }
}
