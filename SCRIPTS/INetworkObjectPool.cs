internal interface INetworkObjectPool
{
    void AcquireInstance();
    void ReleaseInstance();
}