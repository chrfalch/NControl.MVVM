﻿using System;
namespace NControl.XAnimation
{
	/// <summary>
	/// Contains information about an animation 
	/// </summary>
	public class XAnimationInfo
	{
		static int Counter = 0;

		public long Delay { get; set; }
		public long Duration { get; set; }
		public int AnimationId { get; set; }

		public double Scale { get; set; }
		public double Rotate { get; set; }
		public double TranslationX { get; set; }
		public double TranslationY { get; set; }
		public double Opacity { get; set; }
		public bool OnlyTransform { get; set; }

		public XAnimationInfo(): this(null)
		{
			
		}

		public XAnimationInfo(XAnimationInfo prevAnimationInfo) : this(prevAnimationInfo, true)
		{
		}

		public XAnimationInfo(XAnimationInfo prevAnimationInfo, bool keepTransforms)
		{
			AnimationId = Counter++;

			// Delay should always be reset
			Delay = 0;

			// Duration should be inherited
			Duration = prevAnimationInfo != null ? prevAnimationInfo.Duration : 250;

			if (keepTransforms && prevAnimationInfo != null)
			{
				Scale = prevAnimationInfo.Scale;
				Rotate = prevAnimationInfo.Rotate;
				TranslationX = prevAnimationInfo.TranslationX;
				TranslationY = prevAnimationInfo.TranslationY;
				Opacity = prevAnimationInfo.Opacity;
			}
			else
			{
				Reset();
			}
		}

		public void Reset()
		{
			Scale = 1;
			Rotate = 0;
			TranslationX = 0;
			TranslationY = 0;
			Opacity = 1;
		}

		public override string ToString()
		{
			return string.Format("[#{3}: Delay={0}, Duration={1}, Repeat={2}, Scale={4}, Rotate={5}, TranslationX={6}, TranslationY={7}, Opacity={8}]", Delay, Duration, false, AnimationId, Scale, Rotate, TranslationX, TranslationY, Opacity);
		}
	}
}