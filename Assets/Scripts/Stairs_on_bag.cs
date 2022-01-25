using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs_on_bag : MonoBehaviour
{
    public int stairCount;
    
    // Start is called before the first frame update
    void Start()
    {
        stairCount = PlayerController.Current.AllStairs.Count;

    }
    public void AddNewStair()
    {
        stairCount = PlayerController.Current.AllStairs.Count;
        int x = stairCount / 4 +1;
        if ( stairCount % 3 == 1 )
        {
            stairCount = PlayerController.Current.AllStairs.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y + 0.3f * x), transform.localPosition.z);
        }
        else if (stairCount % 3 == 2)
        {
            stairCount = PlayerController.Current.AllStairs.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y + 0.3f * x), transform.localPosition.z-0.4f);
        }
        else if (stairCount % 3 == 0)
        {
            stairCount = PlayerController.Current.AllStairs.Count;
            transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y + 0.3f * x), transform.localPosition.z-0.8f);
        }


    }
    public void UseStairFromBag()
    {
        PlayerController.Current.UseStair(this);
        stairCount = PlayerController.Current.AllStairs.Count;
    }


}
