using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Well : MonoBehaviour
{
    public GameObject player;

    [Header("Interaction")]
    public bool isLowered = false;
    public float interactionRange;
    public Vector2 interactionPos;
    public KeyCode triggerKey;
    public KeyCode exitKey = KeyCode.Escape;
    public GameObject outline;

    [Header("UI Stuff")]
    public GameObject uiPanel;
    public GameObject rollButton;
    public GameObject wheel;
    public TextMeshProUGUI winName;
    public TextMeshProUGUI winDescription;

    [Header("Animations")]
    public Animator anim;
    public string lowerTrigger;
    public string raiseTrigger;

    [Header("Prize Wheel")]
    public GameObject iconParent;
    public GameObject firstIcon;
    public GameObject[] icons;
    public float iconWidth;
    public int iconCount;
    public float startingSpeed;
    public float decelerationSpeed;
    public float speed;
    public float minSpeed;
    public float maxPos;


    void Start()
    {
        // Initialize stuff
        uiPanel.SetActive(false);
        rollButton.SetActive(true);
        wheel.SetActive(false);
        winName.text = "";
        winDescription.text = "";

        // Initialize the icons in the wheel
        Shooting shooting = player.GetComponent<Shooting>();
        int startIndex = Random.Range(0, shooting.Upgrades.Length);
        firstIcon.GetComponent<Upgrade>().upgrade = shooting.Upgrades[startIndex];
        firstIcon.GetComponent<Upgrade>().updateUI();
        firstIcon.GetComponent<Upgrade>().player = shooting;
        icons = new GameObject[iconCount];
        icons[0] = firstIcon;
        for (int i = 1; i < iconCount; i++)
        {
            GameObject icon = Instantiate(firstIcon, Vector3.zero, Quaternion.identity, iconParent.transform);
            icon.transform.localPosition = firstIcon.transform.localPosition + new Vector3(i * iconWidth, 0, 0);
            icon.GetComponent<Upgrade>().upgrade = shooting.Upgrades[(startIndex + i) % shooting.Upgrades.Length];
            icon.GetComponent<Upgrade>().updateUI();
            icon.GetComponent<Upgrade>().player = shooting;
            icons[i] = icon;
        }
    }
    void Update()
    {
        bool skipRolling = false;
        // Check if player is within interaction range and active
        Vector2 wellCenter = transform.position;
        if (player.activeInHierarchy && Vector2.Distance(player.transform.position, wellCenter + interactionPos) < interactionRange)
        {
            outline.SetActive(true);
            // Check if the player presses the trigger key
            if (Input.GetKeyDown(triggerKey) && player.GetComponent<PlayerMovement>().canMove)
            {
                player.GetComponent<PlayerMovement>().canMove = false;
                anim.SetTrigger(lowerTrigger);
            }
        }
        else
        {
            outline.SetActive(false);
        }

        // initialize the ui once lowered
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Lowered") && !isLowered)
        {
            isLowered = true;
            uiPanel.SetActive(true);
        }

        // Raised state
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Raised") && isLowered)
        {
            isLowered = false;
            player.GetComponent<PlayerMovement>().canMove = true;
        }


        // ---- EXIT LOGIC ----
        if (Input.GetKeyDown(exitKey) && speed == 0 && isLowered && !skipRolling)
        {
            Debug.Log("Exiting well");
            ExitWell();
        }
        if (!player.activeInHierarchy)
        {
            ExitWell();
            speed = 0;
            return;
        }

        // Spinning the wheel;
        if (speed > 0)
        {
            iconParent.transform.Translate(Vector3.right * speed * Time.deltaTime);
            speed -= decelerationSpeed * Time.deltaTime;
            if (speed < minSpeed)
            {
                speed = 0;
            }
            int c = 0;
            float posX;
            float newPos;

            // Skipping rolling if the exit key is pressed
            skipRolling = Input.GetKeyDown(exitKey);
            if (skipRolling)
            {
                speed = 0;
                Debug.Log("Skipped rolling");
            }

            foreach (GameObject i in icons)
            {
                posX = (i.transform.localPosition.x + iconParent.transform.localPosition.x);
                if (posX > maxPos)
                {
                    newPos = icons[(c+1) % iconCount].transform.localPosition.x - iconWidth;
                    i.transform.localPosition = new Vector3(newPos, 0, 0);
                }
                c++;
                if (speed == 0)
                {
                    if (Mathf.Abs(posX) < iconWidth / 2)
                    {
                        // Get which prize is won
                        iconParent.transform.localPosition = new Vector3(iconParent.transform.localPosition.x-posX, 0, 0);
                        
                        // update win ui
                        winName.text = i.GetComponent<Upgrade>().upgrade.name;
                        winDescription.text = i.GetComponent<Upgrade>().upgrade.description;

                        // apply the upgrade
                        i.GetComponent<Upgrade>().applyUpgrade();

                        break;
                    }
                }
            }
            if (skipRolling)
            {
                return;
            }
        }
    }

    void ExitWell()
    {
        if (uiPanel.activeInHierarchy)
        {
            anim.SetTrigger(raiseTrigger);
            uiPanel.SetActive(false);
            rollButton.SetActive(true);
            wheel.SetActive(false);
            winName.text = "";
            winDescription.text = "";
        }
    }

    // show the range of the interaction
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactionPos, interactionRange);
    }

    public void SpinDaWheel()
    {
        if (player.GetComponent<Hunger>().foodStored > 0)
        {
            player.GetComponent<Hunger>().foodStored--;
            wheel.SetActive(true);
            rollButton.SetActive(false);
            speed = startingSpeed;
        }
    }
}
