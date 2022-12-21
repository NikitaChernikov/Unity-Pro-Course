using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    float mouseSense = 0.2f;
    [SerializeField] GameObject guns;
    GameObject enemy;
    
    void Update()
    {
        if (!GetComponent<PlayerController>().GetPause())
        {
            //Cursor.lockState = CursorLockMode.Locked; 
            foreach (var touch in Input.touches)
            {
                if (touch.position.x >= Screen.width / 2)
                {
                    float rotateX = touch.deltaPosition.x * mouseSense;
                    float rotateY = touch.deltaPosition.y * mouseSense;

                    Vector3 rotPlayer = transform.rotation.eulerAngles;
                    Vector3 rotGuns = guns.transform.rotation.eulerAngles;
                    rotGuns.x += rotateY;
                    //ограничение поля обзора камеры
                    rotGuns.x = (rotGuns.x > 180) ? rotGuns.x - 360 : rotGuns.x;
                    rotGuns.x = Mathf.Clamp(rotGuns.x, -40, 50);
                    //
                    //rotGuns.x += rotateY;
                    rotGuns.y += rotateX;
                    rotPlayer.y += rotateX;
                    
                    transform.rotation = Quaternion.Euler(rotPlayer);
                    guns.transform.rotation = Quaternion.Euler(rotGuns);
                    if (enemy != null)
                    {
                        enemy.transform.Translate(-rotateX / 3, rotateY / 3, 0);
                    }
                }
            }
        }
    }

    public void FindEnemy(GameObject other)
    {
        enemy = other;
    }

    public void ChangeMouseSensitivity(float count)
    {
        mouseSense = count;
    }

}