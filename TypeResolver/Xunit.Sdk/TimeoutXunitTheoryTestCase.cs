﻿
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using TypeResolver.Internal;
using Xunit.Abstractions;


namespace Xunit.Sdk {

    /// <summary>
    /// Implementation of <see cref="XunitTheoryTestCase"/> that can execute test methods on an STA thread with a timeout.
    /// </summary>
    [Serializable]
    public class TimeoutXunitTheoryTestCase : XunitTheoryTestCase, ITimeoutTestCase {
        private bool useStaThread_;
        private int timeout_;

        /// <summary>Initializes a new instance of the <see cref="TimeoutXunitTheoryTestCase"/> class when deserializing.</summary>
        [EditorBrowsable( EditorBrowsableState.Never )]
        [Obsolete( "Called by the de-serializer", true )]
        public TimeoutXunitTheoryTestCase( ) { }

        /// <inheritdoc cref="XunitTheoryTestCase(IMessageSink,TestMethodDisplay,ITestMethod)"/>
        public TimeoutXunitTheoryTestCase( IMessageSink diagnosticMessageSink, TestMethodDisplay testMethodDisplay, ITestMethod testMethod, int timeout, bool useStaThread )
            : base( diagnosticMessageSink, testMethodDisplay, testMethod ) {
            this.useStaThread_ = useStaThread;
            this.timeout_ = timeout;
        }

        /// <inheritdoc/>
        public virtual bool UseStaThread { get { return this.useStaThread_; } }

        /// <inheritdoc/>
        public int Timeout { get { return this.timeout_; } }

        /// <inheritdoc/>
        public override Task<RunSummary> RunAsync( IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource ) {
            // If test is not using STA or Timeout, return base result.
            if( !this.UseStaThread && this.Timeout < 1 )
                return base.RunAsync( diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource );

            return this.TimeoutRunAsync( messageBus, cancellationTokenSource, ( ) => base.RunAsync( diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource ) );
        }

        /// <inheritdoc/>
        public override void Serialize( IXunitSerializationInfo data ) {
            base.Serialize( data );

            data.SerializeTimeoutTestCase( this );
        }

        /// <inheritdoc/>
        public override void Deserialize( IXunitSerializationInfo data ) {
            base.Deserialize( data );

            data.DeserializeTimeoutTestCase( out this.timeout_, out this.useStaThread_ );
        }
    }

}
