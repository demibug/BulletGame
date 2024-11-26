namespace GameNetwork
{
    public enum NetworkChannelStates
    {
        Closed,
        FailToConnect,
        Connected,
        Disconnected,
        Timeout,
        Error,
        Kicked,
    }
}
