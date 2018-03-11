using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeDrawing : MonoBehaviour
{
    const float linelen = 12;
    const float linewidth = .1f;
    const float timeToDraw = 5f;
    LineRenderer[] lines;
    Transform[] objs;
    [SerializeField]
    Transform startPoint = null;
    [Tooltip("Start rotation, if start point is null.")]
    [SerializeField]
    Vector3 startRotation;
    [Tooltip("Start position, if start point is null.")]
    [SerializeField]
    Vector3 startPosition;
    Vector3[] startingPositions = new Vector3[8];

    IEnumerator Start()
    {
        Vector3 sp;
        Quaternion srot;
        if (startPoint != null)
        {
            sp = startPoint.position;
            srot = startPoint.rotation;
        }
        else
        {
            sp = startPosition;
            srot = Quaternion.Euler(startRotation);
        }
 
        Vector3[] changedir = new Vector3[4] { Vector3.zero, Vector3.forward, Vector3.right, Vector3.back };
        Vector3 lastpos = sp;

        for (int i = 0; i < 4; ++i)
        {
            lastpos = startingPositions[i] = lastpos + srot * changedir[i] * linelen;
            startingPositions[i + 4] = lastpos + srot * Vector3.down * linelen;
        }
        lines = new LineRenderer[12];
        objs = new Transform[12];
        Material lineMat = new Material(Shader.Find("Sprites/Default"));
        LineRenderer l;
        int n;
        for (int i = 0; i < 12; ++i)
        {
            GameObject g = new GameObject();
            l = g.AddComponent<LineRenderer>();
            l.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            l.receiveShadows = false;
            l.sharedMaterial = lineMat;
            lines[i] = l;
            GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Transform nt = newObj.transform;
            objs[i] = nt;
            nt.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            l.startColor = Color.blue;
            l.endColor = Color.blue;
            l.startWidth = l.endWidth = linewidth;
            l.positionCount = 2;
            l.useWorldSpace = false;
            n = i >= 8 ? i - 8 : i;
            l.SetPositions(new Vector3[] { startingPositions[n], startingPositions[n] });
            nt.position = startingPositions[n];
            yield return null;
        }

        for (float i = 0; i <= timeToDraw; i += Time.deltaTime)
        {
            float fract = i / timeToDraw;
            for (n = 0; n < 12; ++n)
            {
                l = lines[n];
                int endindex = n >= 8 ? n - 4 : n == 7 ? 4 : n == 3 ? 0 : n + 1;
                Vector3 nextpos = Vector3.Lerp(l.GetPosition(0), startingPositions[endindex], fract);
                l.SetPosition(1, nextpos);
                objs[n].position = l.transform.position + nextpos;
            }
            yield return null;
        }
        for(n = 0; n < 12; ++n)
        {
            int endindex = n >= 8 ? n - 4 : n == 7 ? 4 : n == 3 ? 0 : n + 1;
            lines[n].SetPosition(1, startingPositions[endindex]);
            if (n < 8) continue;
            Destroy(objs[n].gameObject);
        }
    }
}
