using UnityEngine;

public class AvatarCustom : MonoBehaviour
{
    public Vector3 xrRigPosition;  // The desired local position for the XR Rig
    public Vector3 avatarScale;  // The desired scale for the avatar

    private void Start()
    {
        // Get the XR Rig and set its local position based on the specified value
        Transform xrRig = transform.Find("XR Rig");  // replace "XR Rig" with the name of your XR Rig in the hierarchy
        if (xrRig != null)
        {
            xrRig.localPosition = xrRigPosition;
        }
        else
        {
            Debug.LogError("No XR Rig found in the instantiated player");
        }

        // Find the Avatar and set its local scale
        Transform avatar = transform.Find("Avatar");  // replace "Avatar" with the name of your avatar in the hierarchy
        if (avatar != null)
        {
            avatar.localScale = avatarScale;
        }
        else
        {
            Debug.LogError("No Avatar found in the instantiated player");
        }
    }
}
