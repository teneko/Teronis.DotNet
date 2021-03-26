// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using Teronis.IO.FileLocking;
using Xunit;

namespace Teronis.IO
{
    /// <summary>
    /// XUnit used for testing file locker when used concurrently and nested.
    /// </summary>
    public class FileLockerTests
    {
        private const string LockFilePath = "./lock";

        [Fact]
        public void TestSingleFileLocker()
        {
            var fileLocker = new FileLocker(LockFilePath);
            using var manualResetEvent = new ManualResetEventSlim(false);
            var threadsCount = 3;
            using var countdownEvent = new CountdownEvent(threadsCount);
            var spinWait = new SpinWait();

            var threads = Enumerable.Range(0, threadsCount).Select(x => {
                var thread = new Thread(new ThreadStart(() => {
                    void lockFile()
                    {
                        manualResetEvent.Wait();

                        for (var index = 0; index < 25; index++) {
                            using var lockUse = fileLocker.WaitUntilAcquired();

                            if (index % 7 == 0) {
                                fileLocker.Unlock(lockUse.LockId);
                            }

                            spinWait.SpinOnce();
                        }
                    }

                    lockFile();
                    countdownEvent.Signal();
                })) {
                    Name = x.ToString()
                };

                return thread;
            }).ToList();

            foreach (var thread in threads) {
                thread.Start();
            }

            manualResetEvent.Set();
            countdownEvent.Wait();

            foreach (var thread in threads) {
                thread.Join();
            }

            Assert.True(fileLocker.LocksInUse == 0, "The number of locks in use is not zero.");
        }

        [Fact]
        public void TestSingleFileLockerWithExceptions()
        {
            var fileLocker = new FileLocker(LockFilePath, 1);
            using var manualResetEvent = new ManualResetEventSlim(false);
            var threadsCount = 4;
            using var countdownEvent = new CountdownEvent(threadsCount);
            var spinWait = new SpinWait();
            var random = new Random();

            var threads = Enumerable.Range(0, threadsCount).Select(x => {
                var thread = new Thread(new ThreadStart(() => {
                    void lockFile()
                    {
                        manualResetEvent.Wait();

                        for (var index = 0; index < 10; index++) {
                            FileStream threadWideLockUse = null;

                            if (x == 0 && index % 3 == 0) {
                                threadWideLockUse = FileStreamLocker.Default.WaitUntilAcquired(LockFilePath);
                            }

                            try {
                                using var lockUse = fileLocker.WaitUntilAcquired();
                            } catch {
                                // Ingore intentionally.
                            } finally {
                                threadWideLockUse?.Dispose();
                            }

                            spinWait.SpinOnce();
                        }
                    }

                    lockFile();
                    countdownEvent.Signal();
                })) {
                    Name = x.ToString()
                };

                return thread;
            }).ToList();

            foreach (var thread in threads) {
                thread.Start();
            }

            manualResetEvent.Set();
            countdownEvent.Wait();

            foreach (var thread in threads) {
                thread.Join();
            }

            Assert.True(fileLocker.LocksInUse == 0, "The number of locks in use is not zero.");
        }

        [Fact]
        public void TestMultipleFileLockers()
        {
            using var manualResetEvent = new ManualResetEventSlim(false);
            var threadsCount = 2;
            using var countdownEvent = new CountdownEvent(threadsCount);
            var spinWait = new SpinWait();
            var random = new Random();

            var threads = Enumerable.Range(0, threadsCount).Select(x => {
                var thread = new Thread(new ThreadStart(() => {
                    void lockFile()
                    {
                        manualResetEvent.Wait();

                        for (var index = 0; index < 25; index++) {
                            var fileLocker = new FileLocker(LockFilePath);
                            using var lockUse = fileLocker.WaitUntilAcquired();
                            spinWait.SpinOnce();
                        }
                    }

                    lockFile();
                    countdownEvent.Signal();
                })) {
                    Name = x.ToString()
                };

                return thread;
            }).ToList();

            foreach (var thread in threads) {
                thread.Start();
            }

            manualResetEvent.Set();
            countdownEvent.Wait();

            foreach (var thread in threads) {
                // When joining we want no excpetion to appear.
                thread.Join();
            }
        }
    }
}
