using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour {

    public GameObject target;       //Public variable to store a reference to the player game object
    public GameObject player;       //Public variable to store a reference to the player game object

    public float rot;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    public Vector3 offsetPlayer;
    public Vector2Int side;

    public float camRotationSpeed = 2f;
    public float camSquareFollowSpeed = 0.04f;
    public float camPlayerFollowSpeed = 5f;
    public float delay;

    public float squareMoveTime = 1f;
    public float squareRotateTime = 1f;
    public float levelUpTime = 3f;
    public Ease moveToSquareEase;
    public Ease rotateToSquareEase;
    public Ease moveLevelUpEase;

    Tween moveToSquareTween;
    Tween rotateToSquareTween;
    Tween moveLevelUpTween;
    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        //offset = transform.position - player.transform.position;
        offset = new Vector3(0f, -4.6f, -10f);
        offsetPlayer = new Vector3(0f, 3.5f, -10f);
    }
    public void SetTarget(GameObject g, bool init = false)
    {
        if (init)
        {
            transform.position = g.transform.position + offset;
            transform.eulerAngles = Vector3.zero;
            return;
        }
        //StartCoroutine(SetTargetDelay(g));
        if (moveToSquareTween != null)
            moveToSquareTween.Kill();
        if (rotateToSquareTween != null)
            rotateToSquareTween.Kill();
        moveToSquareTween = transform.DOMove(g.transform.position + offset, squareMoveTime).SetEase(moveToSquareEase);
        rotateToSquareTween = transform.DORotate(Vector3.forward * rot, squareRotateTime).SetEase(rotateToSquareEase);
    }
    IEnumerator SetTargetDelay(GameObject g)
    {
        yield return new WaitForSeconds(delay);
        target = g;
    }
    public void SetOffset()
    {
        if (Mathf.Round(rot) == 0f)
            side = new Vector2Int(0, 1);
        else if (Mathf.Round(rot) == 90f)
            side = new Vector2Int(-1, 0);
        else if (Mathf.Round(rot) == 180f || Mathf.Round(rot) == -180f)
            side = new Vector2Int(0, -1);
        else
            side = new Vector2Int(1, 0);
        offset = new Vector3(-4.6f * side.x, -4.6f * side.y, -10f);
        offsetPlayer = new Vector3(3.5f * side.x, 3.5f * side.y, -10f);
    }
    public void SetFollowPlayer(bool follow, GameObject g = null)
    {
        playerFollow = follow;
        if (follow)
        {
            if (moveLevelUpTween != null)
                moveLevelUpTween.Kill();
            if (g != null)
                moveLevelUpTween = transform.DOMove(g.transform.position + offset, levelUpTime).SetEase(moveLevelUpEase);
        }        
    }
    public bool playerFollow;
    // LateUpdate is called after Update each frame
    /*void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        /*if(playerFollow)
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offsetPlayer, camPlayerFollowSpeed);
        else
            transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, camSquareFollowSpeed);
        Quaternion q = Quaternion.AngleAxis(rot, Vector3.forward);
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, q, Time.deltaTime * camRotationSpeed);
    }*/
}
