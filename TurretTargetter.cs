public class TurretTargetter : MonoBehaviour {

    /** 
     * Transform layout:
     * 
     * Empty, with script
     * Child empty 'yobj'
     * Child of that child, 'xobj'
     * Actual game object.
     *                             **/
    public Transform yobj;
    public Transform xobj;
    public Transform target;
    public float ymin = -30f, ymax = 40f;
    public float xmin = -35f, xmax = 60f;

    bool doTarget = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            doTarget = !doTarget;
        }
        if (doTarget) Retarget();
    }
    void Retarget()
    {
        Vector3 t = Vector3.ProjectOnPlane(target.position - transform.position, transform.up);

        float angle = Vector3.SignedAngle(transform.forward, t, transform.up);
        angle = Mathf.Clamp(angle, ymin, ymax);

        Quaternion rot = Quaternion.Euler(0, angle, 0);
        yobj.localRotation = rot;      

        angle = Vector3.Angle((target.position - xobj.position), yobj.forward);
        angle *= -Mathf.Sign(Vector3.Dot(transform.up, (target.position - xobj.position).normalized));
        angle = Mathf.Clamp(angle, xmin, xmax);

        rot = Quaternion.Euler(angle, 0, 0);
        xobj.localRotation = rot;

        Debug.DrawRay(yobj.position, yobj.forward * Vector3.Distance(yobj.position, t), Color.green);
        Debug.DrawRay(xobj.position, xobj.forward * Vector3.Distance(xobj.position, target.position), Color.blue);
    }
}
