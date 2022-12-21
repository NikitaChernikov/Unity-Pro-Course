using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] List<Transform> checkPointPositions; //��� ����� ������ ����������
    [SerializeField] Transform carPosition; //������� ��������� ������    
    [SerializeField] GameObject checkPointVisual; //������ ParticleSystem
    [SerializeField] GameObject finishParticle;
    [SerializeField] Transform finishPosition;
    GameObject target; //������������� ������
    int countCheckPoint; //����� �������� ��������� �� ������

    // Start is called before the first frame update
    void Start()
    {
        target = Instantiate(checkPointVisual);
        ChangeCheckPoint();
    }

    private void ChangeCheckPoint()
    {
        target.transform.position = checkPointPositions[countCheckPoint].position;
        target.transform.rotation = checkPointPositions[countCheckPoint].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (carPosition.position.z >= target.transform.position.z)
        {
            if (countCheckPoint < checkPointPositions.Count - 1)
            {
                countCheckPoint += 1;
                ChangeCheckPoint();
            }
        }
    }
}
