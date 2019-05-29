using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour {

    public GameObject Camera;
    public GameObject pingPong;
    public UnityEngine.PostProcessing.PostProcessingProfile profile;
    
    UnityEngine.PostProcessing.DepthOfFieldModel DOFModel;
    UnityEngine.PostProcessing.ColorGradingModel CGModel;

    UnityEngine.PostProcessing.DepthOfFieldModel.Settings DOFsetting;
    UnityEngine.PostProcessing.ColorGradingModel.Settings CGsetting;
    public HandShock HS;

    private Vector2 datum = new Vector2(1f, 0.95f);
    private float propotion = 0.9f;

    void Start ()
    {
        profile = Camera.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile;

        DOFModel  = profile.depthOfField;
        CGModel = profile.colorGrading;

        DOFModel.enabled = false;
        CGModel.enabled = false;

        DOFsetting = DOFModel.settings;
        CGsetting = CGModel.settings;

    }
	

	void Update ()
    {

        if (HS.isSkill)
        {
            DOFModel.enabled = true;
            CGModel.enabled = true;

            DOFsetting.focusDistance = propotion * Vector3.Magnitude(Camera.transform.position - pingPong.transform.position) + 0.05f;
            profile.depthOfField.settings = DOFsetting;
            
        }
        else
        {
            DOFModel.enabled = false;
            CGModel.enabled = false;
        }

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    float fstop = DOFsetting.aperture;
        //    DOFsetting.aperture = fstop + 0.1f;
        //    profile.depthOfField.settings = DOFsetting;
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    float fstop = DOFsetting.aperture;
        //    DOFsetting.aperture = fstop - 0.1f;
        //    profile.depthOfField.settings = DOFsetting;
        //}
    }
}
