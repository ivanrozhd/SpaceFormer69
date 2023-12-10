using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayFinding : MonoBehaviour
{ 
  [SerializeField] private LineRenderer _lineRenderer;
      //private Transform[] points;
  [SerializeField] private ObjectBehaviour playerGameObject;
  [SerializeField] private GameObject target;
  [SerializeField] private GameObject player;
  [SerializeField] private Texture[] _textures;
  private int animation;
  private NavMeshPath _currentPath;
  private float fpsCounter;


   private void Awake()
   {
      //_lineRenderer = GetComponent<LineRenderer>();
      _currentPath = new NavMeshPath();
     // _lineRenderer.GetPositions(\)
   }

   public void setUpLine(GameObject player)
   {
      // player.destination = target.transform.position;
      // this.points = points;
      // this.player = player;
   }

   private void Update()
   {
      GameObject gameObject = player.GetComponent<CharacterController>().GetTakenObject();
      if (gameObject != null)
      {
         playerGameObject = gameObject.GetComponent<ObjectBehaviour>();
      }

      if ( playerGameObject != null && !playerGameObject.getTaken())
      {
         _lineRenderer.enabled = false;
      }
      
      if ( playerGameObject != null && playerGameObject.getTaken())
      {
         _lineRenderer.enabled = true;
         fpsCounter += Time.deltaTime;

         if (fpsCounter >= 1f / 30f)
         {
            if (animation == _textures.Length - 1)
            {
               animation = 0;
            }
            animation++;
            _lineRenderer.material.SetTexture("_MainTex",_textures[animation]);
            fpsCounter = 0f;
         }
         var playerNavMeshPos = new NavMeshHit();
         NavMesh.SamplePosition(playerGameObject.transform.position, out playerNavMeshPos, 5.0f, NavMesh.AllAreas);

         var targetNavMeshPos = new NavMeshHit();
         NavMesh.SamplePosition(target.transform.position, out targetNavMeshPos, 5.0f, NavMesh.AllAreas);

         NavMesh.CalculatePath(targetNavMeshPos.position, playerNavMeshPos.position, NavMesh.AllAreas, _currentPath);
      
         _lineRenderer.positionCount = _currentPath.corners.Length;
      
         // Debug.Log(player.path.corners.Length);
         // for (int i = 0; i < player.path.corners.Length; i++)
         // {
         //    // Debug.Log("LOL");
         //    Vector3 pointPosition =
         //       new Vector3(player.path.corners[i].x, player.path.corners[i].y, player.path.corners[i].z);
         //    _lineRenderer.SetPosition(i, pointPosition);
         //    Debug.Log(pointPosition.x + " :X");
         //    Debug.Log(pointPosition.y + " :Y");
         //    Debug.Log(pointPosition.z + " :Z");
         //    Debug.DrawLine(player.transform.position, pointPosition,Color.red);
         //  }
      
      
      
         for (var i = 0; i < _currentPath.corners.Length - 1; i++)
         {
            Vector3 pointPosition =
               new Vector3(_currentPath.corners[i].x, _currentPath.corners[i].y + 0.5f, _currentPath.corners[i].z );
            _lineRenderer.SetPosition(i, pointPosition);
          
           // Debug.DrawLine(_currentPath.corners[i], _currentPath.corners[i + 1], Color.red);
         }
         _lineRenderer.SetPosition(_currentPath.corners.Length - 1, player.transform.position);
      }
      
      
   }
}