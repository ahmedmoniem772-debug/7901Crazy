using VirusX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirusX
{
    public partial class Packets : Form
    {
        public static string Packet = "";
        public Packets()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            clientPackets.Click += new EventHandler(clientPackets_Click);
            serverPackets.Click += new EventHandler(serverPackets_Click);
        }
        void serverPackets_Click(object sender, EventArgs e)
        {
            if (serverPackets.SelectedRows.Count != 0)
            {
                Packet = serverPackets.SelectedRows[0].Cells[3].Value.ToString();
            }
        }
        void clientPackets_Click(object sender, EventArgs e)
        {
            if (clientPackets.SelectedRows.Count != 0)
            {
                Packet = clientPackets.SelectedRows[0].Cells[3].Value.ToString();
            }
        }
        private Queue<byte[]> clientData = new Queue<byte[]>();
        private Queue<byte[]> serverData = new Queue<byte[]>();
        public void AddPacketToClient(byte[] buffer)
        {
            lock (clientData)
            {
                byte[] newBuffer = new byte[buffer.Length];
                for (int i = 0; i < newBuffer.Length; i++)
                    newBuffer[i] = buffer[i];
                clientData.Enqueue(newBuffer);
            }
        }
        public void AddPacketToServer(byte[] buffer)
        {
            lock (serverData)
            {
                byte[] newBuffer = new byte[buffer.Length];
                for (int i = 0; i < newBuffer.Length; i++)
                    newBuffer[i] = buffer[i];
                serverData.Enqueue(newBuffer);
            }
        }
        public void Clear()
        {
            clientPackets.Rows.Clear();
            serverPackets.Rows.Clear();
        }
        private void PacketViewerBoxx_Click(object sender, EventArgs e)
        {

        }

        private void Packets_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.packetsForm = null;
        }
        public static uint[] Read7BitEncodedInt(byte[] buffer)
        {
            List<uint> ptr2 = new List<uint>();

            for (int i = 0; i < buffer.Length; )
            {
                if (i + 2 <= buffer.Length)
                {
                    int tmp = buffer[i++];

                    if (tmp % 8 == 0)
                        while (true)
                        {
                            if (i + 1 > buffer.Length) break;
                            tmp = buffer[i++];
                            if (tmp < 128)
                            {
                                ptr2.Add((uint)tmp);
                                break;
                            }
                            else
                            {
                                int result = tmp & 0x7f;
                                if ((tmp = buffer[i++]) < 128)
                                {
                                    result |= tmp << 7;
                                    ptr2.Add((uint)result);
                                    break;
                                }
                                else
                                {
                                    result |= (tmp & 0x7f) << 7;
                                    if ((tmp = buffer[i++]) < 128)
                                    {
                                        result |= tmp << 14;
                                        ptr2.Add((uint)result);
                                        break;
                                    }
                                    else
                                    {
                                        result |= (tmp & 0x7f) << 14;
                                        if ((tmp = buffer[i++]) < 128)
                                        {
                                            result |= tmp << 21;
                                            ptr2.Add((uint)result);
                                            break;
                                        }
                                        else
                                        {
                                            result |= (tmp & 0x7f) << 21;
                                            result |= (tmp = buffer[i++]) << 28;
                                            ptr2.Add((uint)result);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                }
                else break;
            }
            return ptr2.ToArray();
        }
        public static string GetPacketName(int ID)
        {
            string Name = "N/A";
            if (ID == 2005) Name = "CMsg2ndPsw";
            if (ID == 1542) Name = "CMsgAccountSRP6Ex";
            if (ID == 1102) Name = "CMsgAccountSoftKb";
            if (ID == 2307) Name = "CMsgAchievement";
            if (ID == 2046) Name = "CMsgAction";
            if (ID == 2316) Name = "CMsgActivityTask";
            if (ID == 2105) Name = "CMsgActivityTaskReward";
            if (ID == 2203) Name = "CMsgAllot";
            if (ID == 2059) Name = "CMsgArenicScore";
            if (ID == 2291) Name = "CMsgArenicWitness";
            if (ID == 2343) Name = "CMsgAskBuy";
            if (ID == 2344) Name = "CMsgAskBuyQuery";
            if (ID == 2016) Name = "CMsgAthleteShop";
            if (ID == 2296) Name = "CMsgAuction";
            if (ID == 2145) Name = "CMsgAuctionItem";
            if (ID == 2079) Name = "CMsgAuctionQuery";
            if (ID == 2175) Name = "CMsgAura";
            if (ID == 2225) Name = "CMsgBattleEffectiveness";
            if (ID == 2289) Name = "CMsgBeansExchange";
            if (ID == 2341) Name = "CMsgBeastsInfo";
            if (ID == 2342) Name = "CMsgBeastsOpt";
            if (ID == 2131) Name = "CMsgBenefitsConfig";
            if (ID == 2125) Name = "CMsgBetDiceList";
            if (ID == 2056) Name = "CMsgBetDiceOpt";
            if (ID == 2001) Name = "CMsgBossHarmRanking";
            if (ID == 2229) Name = "CMsgCardsLotteryRankList";
            if (ID == 2034) Name = "CMsgCasinoInteractive";
            if (ID == 2173) Name = "CMsgChangeName";
            if (ID == 2250) Name = "CMsgCheatingProgram";
            if (ID == 2118) Name = "CMsgChipsExpression";
            if (ID == 2220) Name = "CMsgCoatStorage";
            if (ID == 2107) Name = "CMsgCompeteRank";
            if (ID == 1084) Name = "CMsgConfirmKeyLogin";
            if (ID == 1098) Name =
                "CMsgConfirmKeyLoginMobile";
            if (ID == 1052) Name = "CMsgConnect";
            if (ID == 2143) Name = "CMsgContribute";
            if (ID == 2094) Name = "CMsgCrossFlagWar";
            if (ID == 2104) Name = "CMsgCrossFlagWarAltar";
            if (ID == 2129) Name = "CMsgCrossFlagWarFlag";
            if (ID == 2169) Name = "CMsgCrossFlagWarMerit";
            if (ID == 2042) Name = "CMsgCrossFlagWarRank";
            if (ID == 2273) Name = "CMsgCrossRank";
            if (ID == 2202) Name = "CMsgCrossSwitch";
            if (ID == 2224) Name = "CMsgData";
            if (ID == 2292) Name = "CMsgDataArray";
            if (ID == 2188) Name = "CMsgDeadMark";
            if (ID == 2262) Name = "CMsgDetainItemInfo";
            if (ID == 2295) Name = "CMsgDetainItemUpdate";
            if (ID == 2076) Name = "CMsgDisconnect";
            if (ID == 2039) Name = "CMsgDominateTeamName";
            if (ID == 2288) Name =
                "CMsgDominateTeamPopPkName";
            if (ID == 2208) Name = "CMsgDominoInteractive";
            if (ID == 2217) Name = "CMsgDominoLostInfo";
            if (ID == 2043) Name = "CMsgDominoNpcInfo";
            if (ID == 2111) Name = "CMsgDominoResult";
            if (ID == 2189) Name = "CMsgDominoScoreBoard";
            if (ID == 2324) Name = "CMsgDutyMinContri";
            if (ID == 2248) Name = "CMsgElitePKArenic";
            if (ID == 2106) Name = "CMsgElitePKGameRankInfo";
            if (ID == 2249) Name = "CMsgElitePKScore";
            if (ID == 2263) Name = "CMsgEmoticons";
            if (ID == 1059) Name = "CMsgEncryptCode";
            if (ID == 2028) Name = "CMsgEnemyList";
            if (ID == 2323) Name = "CMsgEquipLock";
            if (ID == 2242) Name = "CMsgEquipRefineRank";
            if (ID == 2214) Name =
                "CMsgExchangeInnerStrength";
            if (ID == 2109) Name = "CMsgExchangeShopBuy";
            if (ID == 2317) Name = "CMsgExchangeShopGoods";
            if (ID == 2234) Name = "CMsgExpPool";
            if (ID == 2206) Name = "CMsgFMMatch";
            if (ID == 2315) Name = "CMsgFMRoundRobin";
            if (ID == 2332) Name = "CMsgFactionMatch";
            if (ID == 2244) Name = "CMsgFactionMatchWitness";
            if (ID == 2142) Name = "CMsgFactionRankInfo";
            if (ID == 2049) Name = "CMsgFamily";
            if (ID == 2096) Name = "CMsgFamilyOccupy";
            if (ID == 2294) Name = "CMsgFlower";
            if (ID == 2199) Name = "CMsgFlushExp";
            if (ID == 2139) Name = "CMsgFriend";
            if (ID == 2185) Name = "CMsgFriendInfo";
            if (ID == 2304) Name = "CMsgFuse";
            if (ID == 2274) Name = "CMsgGamblingNpc";
            if (ID == 2337) Name = "CMsgGamblingResult";
            if (ID == 2283) Name = "CMsgGamblingTableInfo";
            if (ID == 2194) Name = "CMsgGamblingTableOpt";
            if (ID == 2099) Name =
                "CMsgGamblingTablePlayerList";
            if (ID == 1350) Name = "CMsgVirusXShutDown";
            if (ID == 2318) Name = "CMsgGameTrend";
            if (ID == 2087) Name = "CMsgGemEmbed";
            if (ID == 2081) Name = "CMsgGlobalLottery";
            if (ID == 2222) Name =
                "CMsgGlobalLotteryRankList";
            if (ID == 2258) Name = "CMsgGodExp";
            if (ID == 2256) Name = "CMsgGoldLeaguePoint";
            if (ID == 2051) Name = "CMsgGuide";
            if (ID == 2200) Name = "CMsgGuideInfo";
            if (ID == 2282) Name = "CMsgHairFaceStorage";
            if (ID == 2333) Name = "CMsgHandBrickInfo";
            if (ID == 2155) Name = "CMsgHangUp";
            if (ID == 2147) Name = "CMsgInnerStrengthInfo";
            if (ID == 2115) Name = "CMsgInnerStrengthOpt";
            if (ID == 2113) Name =
                "CMsgInnerStrengthTotalInfo";
            if (ID == 2299) Name = "CMsgInstance";
            if (ID == 2135) Name = "CMsgInteract";
            if (ID == 2312) Name = "CMsgInvadeWarning";
            if (ID == 2320) Name = "CMsgInviteTrans";
            if (ID == 2137) Name = "CMsgItem";
            if (ID == 2233) Name = "CMsgItemDialog";
            if (ID == 2116) Name = "CMsgItemInfo";
            if (ID == 2275) Name = "CMsgItemInfoEx";
            if (ID == 2218) Name = "CMsgItemRefine";
            if (ID == 2067) Name = "CMsgItemRefineOpt";
            if (ID == 2266) Name = "CMsgItemRefineRecord";
            if (ID == 2134) Name = "CMsgItemStatus";
            if (ID == 5005) Name = "CMsgKOKEnterServer";
            if (ID == 5003) Name = "CMsgKOKMailContent";
            if (ID == 5001) Name = "CMsgKOKMailList";
            if (ID == 5002) Name = "CMsgKOKMailOperation";
            if (ID == 5004) Name = "CMsgKOKTrade";
            if (ID == 5007) Name = "CMsgKokAction";
            if (ID == 5006) Name = "CMsgKokMailOpt";
            if (ID == 5000) Name = "CMsgKokSafeBox";
            if (ID == 2032) Name = "CMsgLeagueAllegianceList";
            if (ID == 2078) Name = "CMsgLeagueBeRob";
            if (ID == 2303) Name = "CMsgLeagueConcubines";
            if (ID == 2267) Name =
                "CMsgLeagueImperialCourtList";
            if (ID == 2009) Name = "CMsgLeagueInfo";
            if (ID == 2297) Name = "CMsgLeagueMemList";
            if (ID == 2157) Name = "CMsgLeagueOpt";
            if (ID == 2089) Name = "CMsgLeagueOrderStatus";
            if (ID == 2335) Name =
                "CMsgLeaguePalaceGuardsList";
            if (ID == 2144) Name = "CMsgLeagueRank";
            if (ID == 2178) Name = "CMsgLeagueRobList";
            if (ID == 2338) Name = "CMsgLeagueRobOpt";
            if (ID == 2216) Name = "CMsgLeagueSynList";
            if (ID == 2021) Name = "CMsgLeagueToken";
            if (ID == 2088) Name = "CMsgLeaveWord";
            if (ID == 2241) Name = "CMsgLogin";
            if (ID == 1213) Name = "CMsgLoginChallengeS";
            if (ID == 2311) Name = "CMsgLoginNotice";
            if (ID == 1214) Name = "CMsgLoginProofC";
            if (ID == 2228) Name = "CMsgLottery";
            if (ID == 2309) Name = "CMsgMagicCoat";
            if (ID == 2083) Name = "CMsgMagicColdTime";
            if (ID == 2239) Name = "CMsgMagicEffect";
            if (ID == 2098) Name = "CMsgMagicEffectTime";
            if (ID == 2101) Name = "CMsgMagicInfo";
            if (ID == 3773) Name = "CMsgMailContent";
            if (ID == 3802) Name = "CMsgMailList";
            if (ID == 2213) Name = "CMsgMapInfo";
            if (ID == 2164) Name = "CMsgMapItem";
            if (ID == 2232) Name = "CMsgMelterOpt";
            if (ID == 2071) Name = "CMsgMelterRankList";
            if (ID == 2023) Name = "CMsgMentorPlayer";
            if (ID == 2057) Name = "CMsgMeteSpecial";
            if (ID == 2068) Name = "CMsgMonsterLive";
            if (ID == 2061) Name = "CMsgMonsterTransform";
            if (ID == 2036) Name = "CMsgName";
            if (ID == 2171) Name = "CMsgNationality";
            if (ID == 2084) Name = "CMsgNpc";
            if (ID == 2210) Name = "CMsgNpcInfo";
            if (ID == 2166) Name = "CMsgNpcInfoEX";
            if (ID == 2148) Name = "CMsgOSAction";
            if (ID == 2269) Name = "CMsgOSConnectInfo";
            if (ID == 2127) Name = "CMsgOSEnter";
            if (ID == 2254) Name = "CMsgOperatingAct";
            if (ID == 2037) Name = "CMsgOperatingActInfo";
            if (ID == 2027) Name = "CMsgOsShop";
            if (ID == 2197) Name = "CMsgOverheadLeagueInfo";
            if (ID == 2073) Name = "CMsgOwnKongRank";
            if (ID == 2082) Name = "CMsgOwnKongfuBase";
            if (ID == 2070) Name =
                "CMsgOwnKongfuImproveFeedback";
            if (ID == 2085) Name =
                "CMsgOwnKongfuImproveSummaryInfo";
            if (ID == 1100) Name = "CMsgPCNum";
            if (ID == 2047) Name = "CMsgPCServerConfig";
            if (ID == 2133) Name = "CMsgPKEliteAction";
            if (ID == 2226) Name = "CMsgPKEliteMatchInfo";
            if (ID == 2231) Name = "CMsgPKEnable";
            if (ID == 2080) Name = "CMsgPackage";
            if (ID == 2072) Name = "CMsgPaint";
            if (ID == 2091) Name = "CMsgPeerage";
            if (ID == 2192) Name = "CMsgPetInfo";
            if (ID == 1075) Name = "CMsgPicKey";
            if (ID == 1074) Name = "CMsgPickeyError";
            if (ID == 2052) Name = "CMsgPigeon";
            if (ID == 2277) Name = "CMsgPigeonQuery";
            if (ID == 2163) Name = "CMsgPing";
            if (ID == 2325) Name = "CMsgPkStatistic";
            if (ID == 2227) Name = "CMsgPlayer";
            if (ID == 2301) Name = "CMsgPlayerAttriInfo";
            if (ID == 2327) Name = "CMsgPlayerResult";
            if (ID == 2321) Name = "CMsgPotHistory";
            if (ID == 2044) Name = "CMsgProcessGoalInfo";
            if (ID == 2063) Name = "CMsgProcessGoalTask";
            if (ID == 2077) Name = "CMsgProcessGoalTaskOpt";
            if (ID == 2177) Name = "CMsgPromotionAct";
            if (ID == 2108) Name = "CMsgPromotionInfo";
            if (ID == 1623) Name = "CMsgProxyEncryptCode";
            if (ID == 1624) Name =
                "CMsgProxyEncryptCodeResponse";
            if (ID == 2191) Name = "CMsgQualifyingDetailInfo";
            if (ID == 2097) Name =
                "CMsgQualifyingFightersList";
            if (ID == 2140) Name =
                "CMsgQualifyingInteractive";
            if (ID == 2093) Name = "CMsgQualifyingRank";
            if (ID == 2293) Name =
                "CMsgQualifyingSeasonRankList";
            if (ID == 2040) Name = "CMsgQuiz";
            if (ID == 2180) Name = "CMsgRaceTrackProp";
            if (ID == 2219) Name = "CMsgRaceTrackPropEffect";
            if (ID == 2215) Name = "CMsgRaceTrackStatus";
            if (ID == 2165) Name = "CMsgRank";
            if (ID == 2054) Name = "CMsgRankMemberShow";
            if (ID == 2025) Name = "CMsgRealItem";
            if (ID == 2029) Name = "CMsgRedEnvelops";
            if (ID == 2110) Name = "CMsgRedeemExp";
            if (ID == 2319) Name = "CMsgRefineEffect";
            if (ID == 2150) Name = "CMsgRegister";
            if (ID == 2160) Name = "CMsgRelation";
            if (ID == 1083) Name = "CMsgRequestKeyLogin";
            if (ID == 2048) Name = "CMsgRoulette1ArgAction";
            if (ID == 2102) Name = "CMsgRouletteAction";
            if (ID == 2045) Name = "CMsgRouletteInvite";
            if (ID == 2013) Name =
                "CMsgRouletteLatestProfitLossList";
            if (ID == 2278) Name = "CMsgRouletteNpcInfo";
            if (ID == 2019) Name = "CMsgRoulettePlayer";
            if (ID == 2090) Name = "CMsgRoulettePlayerBet";
            if (ID == 2030) Name = "CMsgRouletteTable";
            if (ID == 2112) Name = "CMsgRouletteWatcherList";
            if (ID == 2330) Name =
                "CMsgRouletteWinningNumber";
            if (ID == 2286) Name = "CMsgRuneStorage";
            if (ID == 1637) Name = "CMsgSRP6ABgpConnectEx";
            if (ID == 2002) Name = "CMsgSafeHeat";
            if (ID == 2181) Name = "CMsgSelfSynMemAwardRank";
            if (ID == 2161) Name = "CMsgServerInfo";
            if (ID == 2162) Name = "CMsgServerList";
            if (ID == 2020) Name = "CMsgShowHandActivePlayer";
            if (ID == 2114) Name = "CMsgShowHandCallAction";
            if (ID == 2252) Name = "CMsgShowHandDealtCard";
            if (ID == 2195) Name = "CMsgShowHandEnter";
            if (ID == 2151) Name = "CMsgShowHandExit";
            if (ID == 2329) Name = "CMsgShowHandGameResult";
            if (ID == 2246) Name = "CMsgShowHandKick";
            if (ID == 2050) Name = "CMsgShowHandLayCard";
            if (ID == 2340) Name = "CMsgShowHandLostInfo";
            if (ID == 2153) Name = "CMsgShowHandOnlineStatus";
            if (ID == 2328) Name =
                "CMsgShowHandRaceInteractive";
            if (ID == 2121) Name = "CMsgShowHandTrusteeship";
            if (ID == 2253) Name = "CMsgSignIn";
            if (ID == 2184) Name = "CMsgSlotAction";
            if (ID == 2007) Name = "CMsgSlotResult";
            if (ID == 2290) Name = "CMsgSolidify";
            if (ID == 2345) Name = "CMsgSpiritInteractive";
            if (ID == 2008) Name = "CMsgSponsor";
            if (ID == 2026) Name = "CMsgSponsorInfo";
            if (ID == 2174) Name = "CMsgStatisticDaily";
            if (ID == 2033) Name = "CMsgSubPro";
            if (ID == 2336) Name = "CMsgSuitStatus";
            if (ID == 2092) Name = "CMsgSuperFlag";
            if (ID == 2272) Name = "CMsgSynCompete";
            if (ID == 2300) Name = "CMsgSynMemberInfo";
            if (ID == 2259) Name = "CMsgSynMemberList";
            if (ID == 2326) Name =
                "CMsgSynRecruitAdvertising";
            if (ID == 2245) Name =
                "CMsgSynRecruitAdvertisingList";
            if (ID == 2141) Name = "CMsgSyncAction";
            if (ID == 2167) Name = "CMsgSyndicate";
            if (ID == 2172) Name =
                "CMsgSyndicateAttributeInfo";
            if (ID == 2146) Name = "CMsgSynpOffer";
            if (ID == 2261) Name = "CMsgTQP";
            if (ID == 2179) Name = "CMsgTalk";
            if (ID == 2136) Name = "CMsgTaskDetailInfo";
            if (ID == 2255) Name = "CMsgTaskDialog";
            if (ID == 2038) Name = "CMsgTaskReward";
            if (ID == 2120) Name = "CMsgTaskStatus";
            if (ID == 2243) Name = "CMsgTeam";
            if (ID == 2314) Name =
                "CMsgTeamArenaFightingMemberInfo";
            if (ID == 2069) Name =
                "CMsgTeamArenaFightingTeamList";
            if (ID == 2238) Name = "CMsgTeamArenaHeroData";
            if (ID == 2060) Name = "CMsgTeamArenaInteractive";
            if (ID == 2186) Name = "CMsgTeamArenaRank";
            if (ID == 2190) Name = "CMsgTeamArenaScore";
            if (ID == 2287) Name = "CMsgTeamArenaYTop10List";
            if (ID == 2240) Name = "CMsgTeamAward";
            if (ID == 2074) Name = "CMsgTeamMember";
            if (ID == 2270) Name = "CMsgTeamPKArenic";
            if (ID == 2322) Name = "CMsgTeamPKArenicScore";
            if (ID == 2198) Name = "CMsgTeamPKMatchInfo";
            if (ID == 2095) Name = "CMsgTeamPKRankInfo";
            if (ID == 2193) Name = "CMsgTeamPopPKArenic";
            if (ID == 2223) Name = "CMsgTeamPopPKArenicScore";
            if (ID == 2260) Name = "CMsgTeamPopPKMatchInfo";
            if (ID == 2170) Name = "CMsgTeamPopPKRankInfo";
            if (ID == 2285) Name = "CMsgTeamRoll";
            if (ID == 2281) Name =
                "CMsgTenTimesLotteryReward";
            if (ID == 2236) Name =
                "CMsgTexasChampionshipTableChip";
            if (ID == 2103) Name =
                "CMsgTexasExChampionshipList";
            if (ID == 2003) Name =
                "CMsgTexasExChampionshipRank";
            if (ID == 2196) Name = "CMsgTexasExInteractive";
            if (ID == 2308) Name =
                "CMsgTexasExMatchFieldList";
            if (ID == 2331) Name =
                "CMsgTexasExMyChampionshipRank";
            if (ID == 2130) Name = "CMsgTexasHallOfFameList";
            if (ID == 2247) Name =
                "CMsgTexasHallOfFamePlayerList";
            if (ID == 2237) Name = "CMsgTexasInteractive";
            if (ID == 2015) Name = "CMsgTexasNpcInfo";
            if (ID == 2132) Name = "CMsgTexasPersonalInfo";
            if (ID == 2182) Name = "CMsgTexasSeasonState";
            if (ID == 2122) Name = "CMsgTick";
            if (ID == 2279) Name = "CMsgTime";
            if (ID == 2152) Name = "CMsgTitle";
            if (ID == 2313) Name = "CMsgTitleStorage";
            if (ID == 2100) Name = "CMsgTokenUpdate";
            if (ID == 2251) Name = "CMsgTotemPole";
            if (ID == 2271) Name = "CMsgTotemPoleInfo";
            if (ID == 2339) Name = "CMsgTotemsRegister";
            if (ID == 2119) Name = "CMsgTrade";
            if (ID == 2004) Name = "CMsgTradeBuddy";
            if (ID == 2123) Name = "CMsgTradeBuddyInfo";
            if (ID == 2159) Name = "CMsgTraining";
            if (ID == 2117) Name = "CMsgTrainingInfo";
            if (ID == 2075) Name = "CMsgTrainingVitality";
            if (ID == 2268) Name =
                "CMsgTrainingVitalityExpiryNotify";
            if (ID == 2204) Name = "CMsgTrainingVitalityInfo";
            if (ID == 2302) Name =
                "CMsgTrainingVitalityProtect";
            if (ID == 2235) Name =
                "CMsgTrainingVitalityProtectInfo";
            if (ID == 2276) Name =
                "CMsgTrainingVitalityScore";
            if (ID == 2053) Name = "CMsgTransportor";
            if (ID == 2011) Name = "CMsgTurnoverLottery";
            if (ID == 2154) Name = "CMsgUserAbilityScore";
            if (ID == 2176) Name = "CMsgUserAttrib";
            if (ID == 2168) Name = "CMsgUserCityInfo";
            if (ID == 2209) Name = "CMsgUserIPInfo";
            if (ID == 2221) Name = "CMsgUserInfo";
            if (ID == 2128) Name = "CMsgUserTotalRefineLev";
            if (ID == 2024) Name = "CMsgVerifyCheck";
            if (ID == 2284) Name =
                "CMsgVipFunctionValidNotify";
            if (ID == 2126) Name = "CMsgVipUserHandle";
            if (ID == 2205) Name = "CMsgWalk";
            if (ID == 2062) Name = "CMsgWarFlag";
            if (ID == 2041) Name = "CMsgWeaponSkill";
            if (ID == 2010) Name = "CMsgWeaponsInfo";
            if (ID == 2306) Name = "CMsgWeather";
            return Name;
        }
        private void clientPackets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GetBytes(Packet) == null)
                return;
            if (e.RowIndex != -1)
                Packet = clientPackets.Rows[e.RowIndex].Cells[3].Value.ToString();
            PacketViewerGroupBox.Text = "Packet :" + BitConverter.ToUInt16((GetBytes(Packet)), 2) + "  Length :" + BitConverter.ToUInt16((GetBytes(Packet)), 0);
            Be.Windows.Forms.DynamicByteProvider b_collection = new Be.Windows.Forms.DynamicByteProvider(GetBytes(Packet));
            PacketViewerBoxx.ByteProvider = b_collection;
        }
        private void serverPackets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GetBytes(Packet) == null)
                return;
            if (e.RowIndex != -1)
                Packet = serverPackets.Rows[e.RowIndex].Cells[3].Value.ToString();
            PacketViewerGroupBox.Text = "Packet :" + BitConverter.ToUInt16((GetBytes(Packet)), 2) + "  Length :" + BitConverter.ToUInt16((GetBytes(Packet)), 0);
            Be.Windows.Forms.DynamicByteProvider b_collection = new Be.Windows.Forms.DynamicByteProvider(GetBytes(Packet));
            PacketViewerBoxx.ByteProvider = b_collection;
        }
        private byte[] getPacket()
        {
            string text = Packet;
            string[] data = text.Split(new string[] { " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            byte[] buffer = new byte[data.Length + 8];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Length > 2)
                {
                    MessageBox.Show("Invalid value in your packet!");
                    return null;
                }
                buffer[i] = Convert.ToByte(data[i], 16);
            }
            return buffer;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int num = 0, num2 = 0;
            if (comboBox1.Text.Contains("-"))
            {
                string[] spilit = comboBox1.Text.Split('-');
                if (int.TryParse(spilit[0], out num) && int.TryParse(spilit[1], out num2))
                {
                    for (int i = num; i <= num2; i++)
                    {
                        if (comboBox1.Items.Contains(i))
                        {
                            comboBox1.Items.Remove(i);
                        }
                        else if (!comboBox1.Items.Contains(i))
                        {
                            comboBox1.Items.Add(i);
                        }
                    }
                }
            }
            else if (int.TryParse(comboBox1.Text, out num))
            {
                if (comboBox1.Items.Contains(num))
                {
                    comboBox1.Items.Remove(num);
                }
                else if (!comboBox1.Items.Contains(num))
                {
                    comboBox1.Items.Add(num);
                }
            }
            else
            {
                for (ushort i = 1000; i < 3000; i++)
                    if (GetPacketName(i).ToLower() == comboBox1.Text.ToLower())
                    {
                        num = i;
                        if (comboBox1.Items.Contains(num))
                        {
                            comboBox1.Items.Remove(num);
                        }
                        else if (!comboBox1.Items.Contains(num))
                        {
                            comboBox1.Items.Add(num);
                        }
                        break;
                    }
            }
            comboBox1.Text = "";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            clientPackets.Rows.Clear();
            serverPackets.Rows.Clear();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (Packet != null && Packet != "")
            {
                try
                {
                    Clipboard.SetText(Packet);
                }
                catch { }
            }
        }
        public string Dump(byte[] packet)
        {
            try
            {
                ushort PacketLength = BitConverter.ToUInt16(packet, 0);
                if (ASCIIEncoding.ASCII.GetString(packet).Contains("TQServer") || ASCIIEncoding.ASCII.GetString(packet).Contains("TQClient"))
                    PacketLength += 8;
                if (PacketLength > packet.Length) PacketLength = (ushort)packet.Length;
                string DataStr = "";
                var Date = DateTime.Now;
                string Dates = Date.DayOfWeek.ToString() + ", " + Date.Day + ", " + Date.Month + ", " + Date.Year + " / " + Date.Hour + ":" + Date.Minute + ":" + Date.Second + "," + Date.Millisecond;
                DataStr += ";Length : " + PacketLength + ", PacketType: " + BitConverter.ToInt16(packet, 2) + Environment.NewLine + Dates + Environment.NewLine + Environment.NewLine;
                for (int i = 0; i < Math.Ceiling((double)PacketLength / 16); i++)
                {
                    int t = 16;
                    if (((i + 1) * 16) > PacketLength)
                        t = PacketLength - (i * 16);
                    for (int a = 0; a < t; a++)
                    {
                        DataStr += packet[i * 16 + a].ToString("X2") + " ";
                    }
                    if (t < 16)
                        for (int a = t; a < 16; a++)
                            DataStr += "   ";
                    DataStr += "     ;";

                    for (int a = 0; a < t; a++)
                    {
                        DataStr += Convert.ToChar(packet[i * 16 + a]);
                    }
                    DataStr += Environment.NewLine;
                }
                DataStr.Replace(Convert.ToChar(0), '.');

                DataStr += Environment.NewLine;
                DataStr += "=======================================================================";
                DataStr += Environment.NewLine;
                return DataStr;
            }
            catch { }

            return null;

        }
        public string Dump(string packet2)
        {
            try
            {
                byte[] packet = Encoding.ASCII.GetBytes(packet2);
                ushort PacketLength = BitConverter.ToUInt16(packet, 0);
                if (ASCIIEncoding.ASCII.GetString(packet).Contains("TQServer") || ASCIIEncoding.ASCII.GetString(packet).Contains("TQClient"))
                    PacketLength += 8;
                if (PacketLength > packet.Length) PacketLength = (ushort)packet.Length;
                string DataStr = "";
                var Date = DateTime.Now;
                string Dates = Date.DayOfWeek.ToString() + ", " + Date.Day + ", " + Date.Month + ", " + Date.Year + " / " + Date.Hour + ":" + Date.Minute + ":" + Date.Second + "," + Date.Millisecond;
                DataStr += ";Length : " + PacketLength + ", PacketType: " + BitConverter.ToInt16(packet, 2) + Environment.NewLine + Dates + Environment.NewLine + Environment.NewLine;
                for (int i = 0; i < Math.Ceiling((double)PacketLength / 16); i++)
                {
                    int t = 16;
                    if (((i + 1) * 16) > PacketLength)
                        t = PacketLength - (i * 16);
                    for (int a = 0; a < t; a++)
                    {
                        DataStr += packet[i * 16 + a].ToString("X2") + " ";
                    }
                    if (t < 16)
                        for (int a = t; a < 16; a++)
                            DataStr += "   ";
                    DataStr += "     ;";

                    for (int a = 0; a < t; a++)
                    {
                        DataStr += Convert.ToChar(packet[i * 16 + a]);
                    }
                    DataStr += Environment.NewLine;
                }
                DataStr.Replace(Convert.ToChar(0), '.');

                DataStr += Environment.NewLine;
                DataStr += "=======================================================================";
                DataStr += Environment.NewLine;
                return DataStr;
            }
            catch { }

            return null;

        }
        public static byte[] GetBytes(string packetString)
        {
            if (packetString != "" && packetString != null && packetString != string.Empty && packetString != " ")
            {
                byte[] bytes = new byte[(packetString.Length / 3) + 1];
                int t = 0;
                for (int i = 0; i < packetString.Length; i += 3)
                {
                    string cha = packetString.Substring(i, 2);
                    bytes[t] = byte.Parse(cha, System.Globalization.NumberStyles.HexNumber);
                    t++;
                }
                return bytes;
            }
            return null;
        }
        private void button1_Click_3(object sender, EventArgs e)
        {
            try
            {
                char[] Insults = new char[16] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A', 'B', 'C', 'E', 'D', 'F' };
                if (!String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    foreach (var character in textBox1.Text.ToLower())
                    {
                        if (!Insults.Contains(character.ToString().ToUpper().ToCharArray()[0]))
                            textBox1.Text = textBox1.Text.Replace(character.ToString(), "");
                    }
                    decVal.Text = "Decimal: " + ulong.Parse(textBox1.Text.Replace(" ", ""), System.Globalization.NumberStyles.HexNumber);

                    stringVal.Text = "ASCII: " + HexString2Ascii(textBox1.Text.Replace(" ", ""));
                }
                else
                {
                    decVal.Text = "Decimal: ";

                    stringVal.Text = "ASCII: ";
                }
            }
            catch { }
        }
        public string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = "";
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
        private string HexString2Ascii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }
        public static string DumpPacket(byte[] packet)
        {
            string p = "";
            foreach (byte D in packet)
            {
                p += (Convert.ToString(D, 16)).PadLeft(2, '0') + " ";
            }
            return p.ToUpper();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Packet != null && !String.IsNullOrWhiteSpace(Packet) && !String.IsNullOrEmpty(Packet))
            {
                byte x = byte.Parse(textBox2.Text);
                var packet = GetBytes(Packet);
                var packet2 = new byte[packet.Length - x];
                Array.Copy(packet, x, packet2, 0, packet2.Length);
                var variable = Read7BitEncodedInt(packet2).SelectMany(BitConverter.GetBytes).ToArray();
                Be.Windows.Forms.DynamicByteProvider b_collection = new Be.Windows.Forms.DynamicByteProvider(variable);
                PacketViewerBoxx.ByteProvider = b_collection;
            }
            else
            {
                /* var packetWithAdditionals = "08 03 10 D9 36 1A 14 08 33 12 10 46 6C 6F 67 4F 72 44 69 65 00 00 00 00 00 00 00 1A 14 08 35 12 10 4A 75 73 74 2A 46 6F 72 2A 46 75 6E 00 00 00 00 1A 14 08 3A 12 10 47 69 61 6E 74 73 00 00 00 00 00 00 00 00 00 00 1A 14 08 3C 12 10 4C 6F 63 6B 44 6F 77 6E 00 00 00 00 00 00 00 00 1A 14 08 3D 12 10 53 6C 65 65 70 4E F8 4D F8 72 65 00 00 00 00 00 1A 14 08 3F 12 10 4E 65 78 75 73 00 00 00 00 00 00 00 00 00 00 00 1A 14 08 40 12 10 21 5A 21 4F 76 65 72 4C 6F 72 64 00 00 00 00 00";
                 byte x = byte.Parse(textBox2.Text);
                 var packet = GetBytes(packetWithAdditionals);
                 var packet2 = new byte[packet.Length - x];
                 Array.Copy(packet, x, packet2, 0, packet2.Length);
                 var variable = Program.Read7BitEncodedInt(packet2).SelectMany(BitConverter.GetBytes).ToArray();
                 Be.Windows.Forms.DynamicByteProvider b_collection = new Be.Windows.Forms.DynamicByteProvider(variable);
                 PacketViewerBoxx.ByteProvider = b_collection;*/
            }

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1_Click_3(sender, e);
        }
        private void PacketViewerBox_Click(object sender, EventArgs e)
        {

        }

        private void AddPacketID(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button3_Click(sender, null);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ChangeText(object sender, EventArgs e)
        {
            int num = 0, num2 = 0;
            if (comboBox1.Text.Contains("-"))
            {
                string[] spilit = comboBox1.Text.Split('-');
                if (int.TryParse(spilit[0], out num) && int.TryParse(spilit[1], out num2))
                {
                    for (int i = num; i <= num2; i++)
                    {
                        if (comboBox1.Items.Contains(i))
                        {
                            AddFltrBtn.Text = "Remove";
                        }
                        else if (!comboBox1.Items.Contains(i))
                        {
                            AddFltrBtn.Text = "Add";
                        }
                        else
                        {
                            AddFltrBtn.Text = "Add-Remove";
                        }
                    }
                }
                else
                {

                    AddFltrBtn.Text = "Add-Remove";
                }
            }
            else if (int.TryParse(comboBox1.Text, out num))
            {
                if (comboBox1.Items.Contains(num))
                {
                    AddFltrBtn.Text = "Remove";
                }
                else if (!comboBox1.Items.Contains(num))
                {
                    AddFltrBtn.Text = "Add";
                }
            }
            else
            {

                for (ushort i = 1000; i < 3000; i++)
                    if (GetPacketName(i).ToLower() == comboBox1.Text.ToLower())
                    {
                        num = i;
                        if (comboBox1.Items.Contains(num))
                        {
                            AddFltrBtn.Text = "Remove";
                            return;
                        }
                        else if (!comboBox1.Items.Contains(num))
                        {
                            AddFltrBtn.Text = "Add";
                            return;
                        }
                    }
                AddFltrBtn.Text = "Add-Remove";
            }
        }

        private void serverPacket_Up(object sender, KeyEventArgs e)
        {
            if (serverPackets.RowCount > 0)
            {
                if (serverPackets.SelectedRows.Count > 0)
                {
                    int index = serverPackets.SelectedCells[0].OwningRow.Index;
                    if (index != -1)
                        Packet = serverPackets.Rows[index].Cells[3].Value.ToString();
                    PacketViewerGroupBox.Text = "Packet :" + BitConverter.ToUInt16((GetBytes(Packet)), 2) + "  Lenght :" + BitConverter.ToUInt16((GetBytes(Packet)), 0);
                    Be.Windows.Forms.DynamicByteProvider b_collection = new Be.Windows.Forms.DynamicByteProvider(GetBytes(Packet));
                    PacketViewerBoxx.ByteProvider = b_collection;
                }
            }
        }

        private void clientPackets_KeyUp(object sender, KeyEventArgs e)
        {
            if (clientPackets.RowCount > 0)
            {
                if (clientPackets.SelectedRows.Count > 0)
                {
                    int index = clientPackets.SelectedCells[0].OwningRow.Index;
                    if (index != -1)
                        Packet = clientPackets.Rows[index].Cells[3].Value.ToString();
                    PacketViewerGroupBox.Text = "Packet :" + BitConverter.ToUInt16((GetBytes(Packet)), 2) + "  Lenght :" + BitConverter.ToUInt16((GetBytes(Packet)), 0);
                    Be.Windows.Forms.DynamicByteProvider b_collection = new Be.Windows.Forms.DynamicByteProvider(GetBytes(Packet));
                    PacketViewerBoxx.ByteProvider = b_collection;
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void PacketViewerBoxx_SelectionStartChanged(object sender, EventArgs e)
        {
            label1.Text = "Offset :" + PacketViewerBoxx.SelectionStart.ToString();
            int index = int.Parse(PacketViewerBoxx.SelectionStart.ToString());
        }

        private void Packets_Load(object sender, EventArgs e)
        {

        }
        private string toText(byte[] buffer)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var b in buffer)
            {
                builder.Append(b.ToString("X2"));
                builder.Append(" ");
            }
            return builder.ToString();
        }
        public ulong getValue(byte[] buffer, int offset, int length)
        {
            ulong val = 0;
            int left = 0;
            while (left != length)
            {
                val |= ((ulong)buffer[offset]) << (left * 8);
                left++;
                offset++;
            }
            return val;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (clientData.Count != 0)
            {
                lock (clientData)
                {
                    var buffer = clientData.Dequeue();
                    int lenght = BitConverter.ToInt16(buffer, 0);
                    int type = BitConverter.ToInt16(buffer, 2);
                    string types = BitConverter.ToString(buffer, 0);
                    if (checkBox1.Checked)
                    {
                        if (!comboBox1.Items.Contains(type))
                            return;
                    }
                    if (!checkBox1.Checked)
                    {
                        if (comboBox1.Items.Contains(type))
                            return;
                    }
                    clientPackets.Rows.Add();
                    var cells = clientPackets.Rows[clientPackets.Rows.Count - 1].Cells;
                    cells[0].Value = GetPacketName(type);
                    cells[1].Value = type.ToString();
                    cells[2].Value = lenght.ToString();
                    cells[3].Value = toText(buffer);
                    if (ClientAutoScrollBox.Checked)
                    {
                        clientPackets.FirstDisplayedScrollingRowIndex = clientPackets.RowCount - 1;
                    }
                }
            }
            if (serverData.Count != 0)
            {
                lock (serverData)
                {
                    var buffer = serverData.Dequeue();
                    if (buffer.Length >= 2)
                    {
                        int type = BitConverter.ToInt16(buffer, 2);
                        int lenght = BitConverter.ToInt16(buffer, 0);
                        string types = BitConverter.ToString(buffer, 0);
                        if (checkBox1.Checked)
                        {
                            if (!comboBox1.Items.Contains(type))
                                return;
                        }
                        if (!checkBox1.Checked)
                        {
                            if (comboBox1.Items.Contains(type))
                                return;
                        }
                        serverPackets.Rows.Add();
                        var cells = serverPackets.Rows[serverPackets.Rows.Count - 1].Cells;
                        cells[0].Value = GetPacketName(type);
                        cells[1].Value = type.ToString();
                        cells[2].Value = lenght.ToString();
                        cells[3].Value = toText(buffer);
                        if (ServerAutoScrollBox.Checked)
                        {
                            serverPackets.FirstDisplayedScrollingRowIndex = serverPackets.RowCount - 1;
                        }
                    }
                }
            }
        }
    }
}
