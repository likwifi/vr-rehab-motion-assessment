using System.Collections.Generic;

namespace Mediapipe.Unity.Tutorial
{
    public class PoseComplianceSet
    {
        private List<PoseCompliance> poseComplianceSet;

        public PoseComplianceSet(List<PoseCompliance> poseComplianceSet)
        {
            this.poseComplianceSet = poseComplianceSet;
        }

        public void complies(int[][] angels)
        {
            foreach (var compliance in poseComplianceSet)
            {
                compliance.complies(angels);    
            }
            
        }
    }
}