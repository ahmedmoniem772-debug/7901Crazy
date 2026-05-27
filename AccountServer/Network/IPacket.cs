// * Created by AccountServer
// * Copyright © 2020-2021
// * AccountServer - Project

namespace AccountServer.Interfaces
{
    public unsafe interface IPacket
    {
        byte[] ToArray();
        void Deserialize(byte[] buffer);
    }
}