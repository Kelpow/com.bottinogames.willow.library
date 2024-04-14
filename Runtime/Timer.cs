using UnityEngine;

namespace Willow.Library
{
    public struct Timer
    {
        private readonly bool  isRealtime;
        private float __time { get { return isRealtime ? Time.unscaledTime : Time.time; } }
        
        /// <summary>
        /// The duration of the timer.
        /// </summary>
        public readonly float duration;
        
        private float timerStartTime;

        /// <summary>
        /// The time that has passed since the timer was started. This will return more than the duration of the timer, after the timer has run for its duration.
        /// </summary>
        public float time { get { return __time - timerStartTime; } }
        
        /// <summary>
        /// Returns a value from 0 to 1, based on how long the timer has been running.
        /// </summary>
        public float time01 { get { return Mathf.Clamp01(time / duration); } }

        /// <summary>
        /// Returns a value from 0 to 1, based on how long the timer has been running. This will return more than 1 after the timer has run for its duration.
        /// </summary>
        public float time01Unclamped { get { return time / duration; } }

        /// <summary>
        /// The amount of time until the timer stops running.
        /// </summary>
        public float timeRemaining { get { return duration - time; } }

        /// <summary>
        /// Returns a value from 1 to 0, based on how much time is left on the timer.
        /// </summary>
        public float timeRemaining01 { get { return Mathf.Clamp01(timeRemaining / duration); } }

        /// <summary>
        /// Returns a value from 1 to 0, based on how much time is left on the timer. This will return less than 0 after the timer has run for its duration.
        /// </summary>
        public float timeRemaining01Unclamped { get { return timeRemaining / duration; } }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Start() { timerStartTime = __time; }

        /// <summary>
        /// Stops the timer from running imediately.
        /// </summary>
        public void Stop() { timerStartTime = Mathf.Min(timerStartTime, __time - duration); }

        /// <summary>
        /// Stops the timer from running. Treats the timer as if it was never started. 
        /// time and timeRemaining will act as if the timer ended at the beginning of the application.
        /// </summary>
        public void Reset() { timerStartTime = -duration; }

        /// <summary>
        /// Returns true if the timer is running. 
        /// aka: the time passed since the last call of Start() is less than the duration of the timer;
        /// </summary>
        public bool running { get { return time <= duration; } }

        /// <summary>
        /// Returns false only if Start() has never been called, or Reset() was called last, otherwise returns true. Will not be false even if the timer has run for its duration or Stop() was called.
        /// </summary>
        public bool started { get { return timerStartTime != -duration; } }

        /// <summary>
        /// A tool for measuring time.
        /// </summary>
        /// <param name="duration"> The duration of the timer</param>
        /// <param name="isRealtime"> If true, the timer will ignore Time.timeScale</param>
        public Timer(float duration, bool isRealtime = false) 
        {
            if (duration <= 0)
                throw new System.Exception("A timers duration cannot be less than 0");
            this.duration = duration; 
            this.isRealtime = isRealtime; 
            timerStartTime = -duration; 
        }

        public static implicit operator Timer(float f) { return new Timer(f); }

        public override string ToString()
        {
            return $"Duration: {duration}, Time Elapsed: {time}, Time Remaining: {timeRemaining}";
        }
    }
}