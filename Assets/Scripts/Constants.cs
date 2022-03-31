using System;
using System.Collections.Generic;

public class Constants
{
    public static String LoginScene = "Login";
    public static String CityScene = "City";
    public static String LobbyScene = "Lobby";
    public static String ConcertScene = "Concert";
    public static String JapanScene = "Japan";

    public static String Daryl = "Daryl";
    public static String Varick = "Varick";
    public static String Zavier = "Zavier";
    public static String YewWee = "YewWee";
    
    public static String YewWeeWallet = "4Lz2j3Ga6NeNfiUWfu3FakkVppcfuLsAYMLwq2ELbL6B";
    
    
    public static Dictionary<string, PlayersInfo> WalletToPlayersInfosMap = new Dictionary<string, PlayersInfo>
    {
        {
            "Eqi3aPp3EdTAs9puh4Ze4E5mJ2onzSSr9F94uuZz95qX", new PlayersInfo
            {
                Name = Daryl,
                CharAddr = "4g1MawHaX88JVp9QESDakq7JNH4ZtKAUr9qDTRizRUef"
            }
        },
        {
            "EGuJ8jdrQKyXnSWgZqa2SLFWfzPyPEMeorsZ4GjdejqH", new PlayersInfo
            {
                Name = Zavier,
                CharAddr = "4zbk7BCgmwsRPxsqKXA2kp8g29kVhRnWAgGmrDHtVgQ1"
            }
        },{
            "4Lz2j3Ga6NeNfiUWfu3FakkVppcfuLsAYMLwq2ELbL6B", new PlayersInfo
            {
                Name = YewWee,
                CharAddr = "2s6iAvdspq3zCEzncayTbMn1x5xzFwYWe5To1ZacXU6w"
            }
        },{
            "DhJBRj4keddt3wKVMdsNxBduRZEp2BJeexCAz33CUhqL", new PlayersInfo
            {
                Name = Varick,
                CharAddr = "8uvtX1R7JFGaCYP9tWrWppHvSH2f3tb6pC7tUH6sSqnn"
            }
        },
    };
}