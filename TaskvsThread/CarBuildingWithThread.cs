using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskvsThread.component;

namespace TaskvsThread
{
    public class CarBuildingWithThread : CarBuilding
    {
        public static void Build(int horsePower, int suspensionCount)
        {
            var carBuilding = new CarBuildingWithThread();

            Body body = null!;

            var bodyThread = new Thread(() =>
            {
                body = carBuilding.BuildBody(100, 5, 2);
            });

            bodyThread.Start();

            Engine engine = null!;

            var engineThread = new Thread(() =>
            {
                engine = carBuilding.BuildEngine(horsePower);
            });

            engineThread.Start();

            var suspensions = new ConcurrentBag<Suspension>();

            var suspensionThreads = Enumerable
                                    .Range(0, suspensionCount)
                                    .Select(i =>
                                              {
                                                  var t = new Thread(() =>
                                                  {
                                                      suspensions.Add(carBuilding.BuildSuspension(1));
                                                  });
                                                  t.Start();
                                                  return t;
                                              });

            bodyThread.Join();

            Painting painting;

            var paintingThread = new Thread(() => {
                painting = carBuilding.Paint("red", body.Width * body.Length);
            });

            paintingThread.Start();
            engineThread.Join();

            Exception? thrownException = null;

            var testing = new Thread(() => {
                try
                {
                    carBuilding.Test(body, suspensions, engine);
                }
                catch(Exception exc)
                {
                    thrownException = exc;
                }
            });

            testing.Start();
            testing.Join();

            if(thrownException is not null) throw thrownException;
            paintingThread.Join();

        }

        protected override Body BuildBody(int weight, int length, int width)
        {
            Thread.Sleep(CarBuilding.BODY_BUILD_TIME);
            return base.BuildBody(weight, length, width);
        }

        protected override Engine BuildEngine(int horsePower)
        {
            Thread.Sleep(CarBuilding.ENGINE_BUILD_TIME);
            return base.BuildEngine(horsePower);
        }

        protected override Suspension BuildSuspension(int supportedKg)
        {
            Thread.Sleep(CarBuilding.SUSPENSION_BUILD_TIME);
            return base.BuildSuspension(supportedKg);
        }

        protected override Painting Paint(string color, int bodyArea)
        {
            Thread.Sleep(CarBuilding.PAINTING_TIME);
            return base.Paint(color, bodyArea);
        }

        protected override void Test(Body body, IEnumerable<Suspension> suspensions, Engine engine)
        {
            Thread.Sleep(CarBuilding.TESTING_TIME);
            base.Test(body, suspensions, engine);
        }

    }

    
}
