// Copyright 2014 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;

public class CardboardHead_vL : MonoBehaviour {
  // Which types of tracking this instance will use.
  public bool trackRotation = true;
  public bool trackPosition = true;

  // If set, the head transform will be relative to it.
  public Transform target;

	// target for rotation
	public Transform targetRot;

  // Determine whether head updates early or late in frame.
  // Defaults to false in order to reduce latency.
  // Set this to true if you see jitter due to other scripts using this
  // object's orientation (or a child's) in their own LateUpdate() functions,
  // e.g. to cast rays.
  public bool updateEarly = false;

	//---------------------------------------------------------
  // Where is this head looking?
  public Ray Gaze {
    get {
      UpdateHead();
      return new Ray(transform.position, transform.forward);
    }
  }

  private bool updated;

  void Update() {
    updated = false;  // OK to recompute head pose.
    if (updateEarly) {
      UpdateHead();
    }
  }

  // Normally, update head pose now.
  void LateUpdate() {
    UpdateHead();
  }

  // Compute new head pose.
  private void UpdateHead() {
    if (updated) {  // Only one update per frame, please.
      return;
    }
    updated = true;
//    Cardboard.SDK.UpdateState();
		GvrViewer.Instance.UpdateState();

    if (trackRotation) {
//      var rot = Cardboard.SDK.HeadPose.Orientation;
			var rot = GvrViewer.Instance.HeadPose.Orientation;

			//original
			//doesn't fit my need because I changed the CardboardHead codes
			//so it causes loop rotating
//		      if (target == null) {
//		        transform.localRotation = rot;
//		      } else {
//		        transform.rotation = rot * target.rotation;
//		      }

			//laura
			if (target == null) {
				transform.localRotation = rot;
			} else {
//				transform.localRotation = rot;

//				transform.rotation = rot * targetRot.rotation;
				transform.rotation = targetRot.rotation * rot;
//				transform.rotation = rot * Quaternion.Euler(0, targetRot.rotation.eulerAngles.y, 0);
			}
    }

    if (trackPosition) {
//      Vector3 pos = Cardboard.SDK.HeadPose.Position;
			Vector3 pos = GvrViewer.Instance.HeadPose.Position;

      if (target == null) {
        transform.localPosition = pos;
      } else {
        transform.position = target.position + target.rotation * pos;
      }
    }
  }
}
