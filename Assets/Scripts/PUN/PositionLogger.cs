using UnityEngine;
using System.Collections;

public class PositionLogger : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (true)
        {
            Debug.Log("Current local position of " + gameObject.name + " is " + transform.localPosition);
            yield return new WaitForSeconds(1f);
        }
    }
}
