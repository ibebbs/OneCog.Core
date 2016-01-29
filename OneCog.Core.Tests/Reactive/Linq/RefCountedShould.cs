using NUnit.Framework;
using OneCog.Core.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Core.Tests.Reactive.Linq
{
    [TestFixture]
    public class RefCountedShould
    {
        [Test]
        public void OnlyInstantiateTheResourceOnFirstSubscription()
        {
            int instantiations = 0;

            RefCounted<int> refCounted = new RefCounted<int>(() => ++instantiations, value => --instantiations);

            using (Observables.UsingRefCounted(refCounted, value => Observable.Never<int>()).Subscribe())
            {
                Assert.That(instantiations, Is.EqualTo(1));
            }
        }

        [Test]
        public void DisposeTheResourceOnSubscriptionDisposal()
        {
            int instantiations = 0;

            RefCounted<int> refCounted = new RefCounted<int>(() => ++instantiations, value => --instantiations);

            using (Observables.UsingRefCounted(refCounted, value => Observable.Never<int>()).Subscribe())
            {
                Assert.That(instantiations, Is.EqualTo(1));
            }

            Assert.That(instantiations, Is.EqualTo(0));
        }

        [Test]
        public void NotInstantiateTheResourceOnSubsequentSubscriptions()
        {
            int instantiations = 0;

            RefCounted<int> refCounted = new RefCounted<int>(() => ++instantiations, value => --instantiations);

            IObservable<int> observable = Observables.UsingRefCounted(refCounted, value => Observable.Never<int>());

            using (observable.Subscribe())
            {
                using (observable.Subscribe())
                {
                    Assert.That(instantiations, Is.EqualTo(1));
                }
            }
        }

        [Test]
        public void DisposeTheResourceOnListSubscriptionsDisposal()
        {
            int instantiations = 0;

            RefCounted<int> refCounted = new RefCounted<int>(() => ++instantiations, value => --instantiations);

            IObservable<int> observable = Observables.UsingRefCounted(refCounted, value => Observable.Never<int>());

            using (observable.Subscribe())
            {
                using (observable.Subscribe())
                {
                    Assert.That(instantiations, Is.EqualTo(1));
                }

                Assert.That(instantiations, Is.EqualTo(1));
            }

            Assert.That(instantiations, Is.EqualTo(0));
        }

        [Test]
        public void OnlyInstantiateTheDisposableResourceOnFirstSubscription()
        {
            int instantiations = 0;

            IDisposable disposable = Disposable.Create(() => --instantiations);

            Func<IDisposable> build = () =>
            {
                ++instantiations;

                return disposable;
            };

            using (Observables.UsingRefCounted(build, value => Observable.Never<int>()).Subscribe())
            {
                Assert.That(instantiations, Is.EqualTo(1));
            }
        }

        [Test]
        public void DisposeTheDisposableResourceOnSubscriptionDisposal()
        {
            int instantiations = 0;

            IDisposable disposable = Disposable.Create(() => --instantiations);

            Func<IDisposable> build = () =>
            {
                ++instantiations;

                return disposable;
            };

            using (Observables.UsingRefCounted(build, value => Observable.Never<int>()).Subscribe())
            {
                Assert.That(instantiations, Is.EqualTo(1));
            }

            Assert.That(instantiations, Is.EqualTo(0));
        }

        [Test]
        public void NotInstantiateTheDisposableResourceOnSubsequentSubscriptions()
        {
            int instantiations = 0;

            IDisposable disposable = Disposable.Create(() => --instantiations);

            Func<IDisposable> build = () =>
            {
                ++instantiations;

                return disposable;
            };

            IObservable<int> observable = Observables.UsingRefCounted(build, value => Observable.Never<int>());

            using (observable.Subscribe())
            {
                using (observable.Subscribe())
                {
                    Assert.That(instantiations, Is.EqualTo(1));
                }
            }
        }

        [Test]
        public void DisposeTheDisposableResourceOnLastSubsequentDisposal()
        {
            int instantiations = 0;

            IDisposable disposable = Disposable.Create(() => --instantiations);

            Func<IDisposable> build = () =>
            {
                ++instantiations;

                return disposable;
            };

            IObservable<int> observable = Observables.UsingRefCounted(build, value => Observable.Never<int>());

            using (observable.Subscribe())
            {
                using (observable.Subscribe())
                {
                    Assert.That(instantiations, Is.EqualTo(1));
                }

                Assert.That(instantiations, Is.EqualTo(1));
            }

            Assert.That(instantiations, Is.EqualTo(0));
        }
    }
}
