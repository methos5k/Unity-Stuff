public class LittleSpriteCoupe : MonoBehaviour
{
    [SerializeField]
    Transform car;
    [SerializeField]
    Transform cam;
    Camera mainCam;
    Vector3 targetPos;
    [SerializeField]
    TrailRenderer[] trails;
    bool movingToPosition = false;
    float speed = 0;
    void Start()
    {
        mainCam = cam.GetComponent<Camera>();
    }
 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            movingToPosition = true;
            Vector3 mp = Input.mousePosition;
            mp.z = Mathf.Abs(car.position.z - cam.position.z);
            mp = mainCam.ScreenToWorldPoint(mp);
            targetPos = mp;
        }
        if (movingToPosition)
        {
            Vector3 dir = targetPos - transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);
            float off = Vector3.Dot(dir.normalized, car.right);

            if (off < -.5f) speed = .5f;
            else if (off < 0)
            {
                speed += Time.deltaTime;
                if (speed > 1) speed = 1;
            }
            else if (off < .55f)
            {
                speed += Time.deltaTime * 2f;
                if (speed > 2) speed = 2;
            }
            else
            {
                speed += Time.deltaTime;
                if (speed > 3) speed = 3;
            }
          
            car.rotation = Quaternion.RotateTowards(car.rotation, rot, 180 * Time.deltaTime);
            car.position = car.position + car.right * speed * 5* Time.deltaTime;

            float distsqr = (targetPos - transform.position).sqrMagnitude;
            if (distsqr < .05f)
            {
                speed = 0;
                movingToPosition = false;
            }
        }
    }

}
