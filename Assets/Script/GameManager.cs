using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    None,
    Red,
    Yellow,
    Blue
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [NonSerialized]
    public GameObject playerPrefab;
    private GameObject playerInstance;

    private Rigidbody playerRigid;
    public GameObject redPrefab;
    public GameObject yellowPrefab;
    public GameObject bluePrefab;

    private Vector3 initPosition;
    private Quaternion initRotation;
    private Character currentchar;
    
    private GameObject selectedPrefab;
    private PlayerController playerController;
    public Trajectory trajectory;
    private StateCheckCamera _stateCheckCamera;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        initPosition = new Vector3(0.332f, 1.735f, 0f);
        initRotation = Quaternion.Euler(-50.972f, 90f, 0f);
        if (playerPrefab == null)
        {
            playerPrefab = Resources.Load<GameObject>("BlueBird");
        }
        playerRigid = playerPrefab.GetComponent<Rigidbody>();
        playerController = playerPrefab.GetComponent<PlayerController>();
        
        playerRigid.constraints = RigidbodyConstraints.FreezeAll;

        currentchar = Character.Blue;
        
        SetCurrentPrefab();
        CreateNewPlayer();
    }

    void SetCurrentPrefab()
    {
        selectedPrefab = null;
        switch (currentchar)
        {
            case Character.Red:
                selectedPrefab = redPrefab;
                break;
            case Character.Yellow:
                selectedPrefab = yellowPrefab;
                break;
            case Character.Blue:
                selectedPrefab = bluePrefab;
                break;
        }
    }

    void CreateNewPlayer()
    {
        if (selectedPrefab == null)
        {
            selectedPrefab = bluePrefab;
        }
        else
            Debug.LogError("Not Found selectedPrefab");
            
        Debug.Log("GameManager에서 player 인스턴스 생성함.");
        playerInstance = Instantiate(selectedPrefab, initPosition, initRotation);
        Debug.Log(playerInstance);
        playerInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);// 초기 위치로 돌아감
        playerInstance.transform.position = initPosition;
    }
    
    public GameObject ReturnNewPlayer()
    {
        CreateNewPlayer();
        return playerInstance;
    }
    
    private void Update()
    {
        if (!playerController.bSnapped && !playerController.isThrown)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (playerController.playerPrefab != null)
                {
                    Destroy(playerController.gameObject);
                }
                
                currentchar = Character.Red;
                SetCurrentPrefab();
                CreateNewPlayer();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (playerController.gameObject != null)
                {
                    Destroy(playerController.gameObject);
                }
                currentchar = Character.Yellow;
                SetCurrentPrefab();
                CreateNewPlayer();
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (playerController.gameObject != null)
                {
                    Destroy(playerController.gameObject);
                }
                currentchar = Character.Blue;
                SetCurrentPrefab();
                CreateNewPlayer();
            }
        }
    }

    public void ShowTrajectory(Vector3 startPoint, Vector3 force, float mass)
    {
        trajectory.ShowTrajectory(startPoint, force, mass);
    }

    public void DestroyTrajectory()
    {
        trajectory.DestroyTrajectory();
    }
}
