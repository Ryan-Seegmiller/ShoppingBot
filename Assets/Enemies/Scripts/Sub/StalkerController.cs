using enemymanager;
using Items;
using System.Collections.Generic;
using UnityEngine;

public class StalkerController : EnemyBase
{
    private AnimationCurve BaseCurve =new AnimationCurve();
    private List<Vector3> currentAttackCurve=null;
    private int frame = 0;
    private float lastAttackTime = 0;
    public GameObject propBody; 
    public void Start()
    {
        BaseCurve.AddKey(0, 0);
        BaseCurve.AddKey(1, 0);//attack curve init
        health = 3;
        rb.AddForce(Vector3.up * 50);//push to roof
    }
    new void FixedUpdate()
    {
        bool paused = EnemyManager.instance.PauseEnemies;
        if (!paused)
        {
            if (paused != wasPausedLastFrame)//if the player just unpaused, reset vel to cached value
            {
                UnPausePhysics();
            }
            propBody.transform.Rotate(0, 0, 15);
            base.FixedUpdate();

            if (hasFoundPlayer && currentAttackCurve.Count < 1 && lastAttackTime + 3 < time) { BeginAttackCurve(); }     //if found player, create attack curve            
            if (currentAttackCurve.Count > 0) { RunAttackCurvePhysics(); }  //if there is an attack curve, run it
            else
            {
                DoWander();
            }
            if (hasFoundPlayer && currentDistanceToPlayer < 2f)
            {
                anim.SetTrigger("action");
                //take items from player
                if (ItemManager.instance != null)
                    ItemManager.instance.RemoveRandomItem();
            }
        }
        else if (paused != wasPausedLastFrame)//if the player just paused for the first time, store its velocity, and set to 0
        {
            PausePhysics();
        }
        wasPausedLastFrame = paused;
    }
    void BeginAttackCurve()
    {
        //reset and ready curve list
        lastAttackTime = time;
        Vector3 v = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(v);
        currentAttackCurve.Clear();
        frame = 0;
        currentAttackCurve = CalculateAttackCurve(BaseCurve, transform.position, (transform.position + transform.forward * (2 * Vector3.Distance(transform.position, v))), 30, player.transform.position);
    }
    void RunAttackCurveTranslate()
    {
        //call repeatedly until currentAttackCurve clear, which happens at ELSE
        if (frame < currentAttackCurve.Count)
        {
            transform.position = currentAttackCurve[frame];
            frame++;
        }
        else
            currentAttackCurve.Clear();
    }    //unused
    void RunAttackCurvePhysics()
    {
        //call repeatedly until currentAttackCurve clear, which happens at ELSE
        if (frame < currentAttackCurve.Count)
        {
            transform.LookAt(currentAttackCurve[frame]);
            rb.AddForce(transform.forward * 35);
            frame++;
        }
        else
            currentAttackCurve.Clear();
    }
    static List<Vector3> CalculateAttackCurve(AnimationCurve baseCurve, Vector3 origin, Vector3 target, float steps, Vector3 centerPoint)
    {
        //calculate a swooping curve between origin and target, using basecurve as the curve.
        AnimationCurve curve = new AnimationCurve();
        //base curve holds beggining and end keyframes for curve
        for(int b=0;b<baseCurve.keys.Length; b++)
        {
            curve.AddKey(baseCurve.keys[b]);
        }
        //add middle keyframe here for a negative slope.     ex. base keyframe at (0,0), new keyframe at (0.5,-5), base keyframe at (0,1). the new keyframe is in the middle(0.5f), and 5 down
        curve.AddKey(0.5f, -(origin.y-(centerPoint.y+1)));

        List<Vector3> output=new List<Vector3>();
        float slopeMod = 1 / steps;
        //with steps being the degree of accuracy between movements, fill a list of all the positions along determined curve
        int i = 0;
        for (i=1; i< steps; i++)
        {
            output.Add((Vector3.Lerp(origin, target, (1/steps)*i)) + (Vector3.up*(curve.Evaluate(slopeMod * i))));
        }
        //return list full of curve cordinates
        output.Add(new Vector3(target.x,origin.y,target.z));
        return output;
    }
}
