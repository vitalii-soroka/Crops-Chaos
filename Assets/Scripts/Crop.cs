using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D; 
using UnityEngine.TextCore.Text;

public class Crop : MonoBehaviour
{
    
    private SpriteSwapper _spriteSwapper;

    [SerializeField] private Dropable drop;

    [SerializeField] private int iterations = 3;
    private int currentIteration = 0;

    [SerializeField] private float timeToGrow = 1.0f;

    private float timeSinceLastGrow = 0.0f;

    [SerializeField] string spriteMapBaseName = "Test_Plant_";

    void Start()
    {
        _spriteSwapper = GetComponent<SpriteSwapper>(); 
        if(_spriteSwapper != null )
        {
            _spriteSwapper.ChangeSprite(spriteMapBaseName + currentIteration);
        }
    }

    private void FixedUpdate()
    {
        timeSinceLastGrow += Time.fixedDeltaTime;

        if(currentIteration < iterations && timeSinceLastGrow > timeToGrow)
        {
            timeSinceLastGrow = 0.0f;

            if (_spriteSwapper != null)
            {
                _spriteSwapper.ChangeSprite(spriteMapBaseName + ++currentIteration);
            }
        }
    }
    
    public bool IsGrown()
    {
        return currentIteration == iterations;
    }

    public void BreakCrop()
    {
        // TODO
    }
}
