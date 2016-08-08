using UnityEngine;
using System;
using System.Collections.Generic;

public class SessionPictureInfo : MonoBehaviour
{
    public ListVisibleTargets listVisibleTargets;

    [Serializable]
    public class PictureInfo
    {
        public Collider sMainTarget;
        public bool sFacing;
        public int sNumberTargets;
        public List<Collider> sVisibleTargets;
        public Texture2D sSnapshot;
        

        public PictureInfo(Collider MainTarget, bool Facing, int NumberOfTargets, List<Collider> VisibleTargets, Texture2D Snapshot)
        {
            sMainTarget = MainTarget;
            sFacing = Facing;
            sNumberTargets = NumberOfTargets;
            sVisibleTargets = VisibleTargets;
            sSnapshot = Snapshot;
        }
    }
}