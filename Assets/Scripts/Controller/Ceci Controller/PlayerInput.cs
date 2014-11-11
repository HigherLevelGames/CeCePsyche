using System;
using UnityEngine;
public class PlayerInput : MonoBehaviour
{
    private MovementController controller;
    void Awake()
    {
        controller = GetComponent<MovementController>();
    }
    void Update()
    {
        // Left and Right Stuff
        int rawH = RebindableInput.GetAxis("Horizontal");
        controller.Right = rawH > 0;
        controller.Left = rawH < 0;
        if(controller.Right || controller.Left) // check needed in case standing still
        {
            controller.isFacingRight = controller.Right;
        }
        
        // Ladder Stuff
        int rawV = RebindableInput.GetAxis("Vertical");
        controller.Up = rawV > 0;
        controller.Down = rawV < 0;
        
        // Jumping Stuff
        bool pressJump = RebindableInput.GetKeyDown("Jump");
        controller.UpPress = controller.Up || pressJump;
        controller.UpHold = controller.UpPress && (controller.PrevUp == controller.UpPress);
        controller.UpRelease = !controller.UpPress && controller.PrevUp;
    }
}

