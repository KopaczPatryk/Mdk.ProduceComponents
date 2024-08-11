using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript {
    class CustomTicker {
        private const int TicksInSecond = 60;
        // TODO onstart
        public event Action Every10;
        public event Action Every100;
        public event Action Every1000;
        public event Action EverySeconds;
        public event Action Every5Seconds;
        public event Action Every30Seconds;
        public event Action EveryMinute;

        private int currentTick = 0;

        public CustomTicker() { }

        public void Tick() {
            if(currentTick % 10 == 0) Every10?.Invoke();
            if(currentTick % 100 == 0) Every100?.Invoke();
            if(currentTick % 1000 == 0) Every1000?.Invoke();
            if(currentTick % TicksInSecond == 0) EverySeconds?.Invoke();
            if(currentTick % (TicksInSecond * 5) == 0) Every5Seconds?.Invoke();
            if(currentTick % (TicksInSecond * 30) == 0) Every30Seconds?.Invoke();
            if(currentTick % (TicksInSecond * 60) == 0) EveryMinute?.Invoke();

            currentTick++;
        }
    }
}
