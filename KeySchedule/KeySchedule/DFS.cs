using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KeySchedule
{
    class DFS
    {
        const uint ONE = 0b0001;
        const int OPEN = 1, CLOSE = 3;

        int[][] schedule;

        public DFS()
        {
            schedule = new int[][]{
                new int[] { 2, 2, 2, 0, 2, 0, 2, 2, 2, 2 },
                new int[] { 0, 3, 2, 2, 3, 0, 3, 0, 1, 2 },
                new int[] { 2, 0, 2, 0, 2, 2, 2, 2, 0, 1 },
                new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 3, 0 },
                new int[] { 3, 0, 3, 0, 0, 0, 0, 3, 0, 3 },
                new int[] { 0, 0, 3, 0, 0, 3, 0, 0, 0, 3 },
                new int[] { 0, 3, 0, 0, 0, 0, 0, 0, 3, 0 },
                new int[] { 3, 0, 0, 3, 0, 0, 0, 3, 0, 0 },
                new int[] { 0, 0, 0, 3, 0, 0, 3, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 3, 3, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 3, 0, 0, 0, 0, 0, 0 },
                new int[] { 1, 1, 0, 1, 1, 2, 0, 1, 0, 0 },
                new int[] { 1, 0, 1, 0, 0, 1, 1, 1, 0, 1 },
                new int[] { 0, 1, 0, 0, 1, 1, 1, 0, 1, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            /*schedule = new int[][]{
                new int[] { 2, 2, 2, 0, 2},
                new int[] { 0, 3, 2, 2, 3},
                new int[] { 2, 0, 2, 0, 2 },
                new int[] { 0, 0, 0, 1, 0 },
                new int[] { 3, 0, 3, 0, 0 },
                new int[] { 0, 0, 3, 0, 0 },
                new int[] { 0, 3, 0, 0, 0 },
                new int[] { 3, 0, 0, 3, 0 },
                new int[] { 0, 0, 0, 3, 0 },
                new int[] { 0, 0, 0, 0, 3},
                new int[] { 0, 0, 0, 3, 0},
                new int[] { 1, 1, 0, 1, 1 },
                new int[] { 1, 0, 1, 0, 0},
                new int[] { 0, 1, 0, 0, 1 },
                new int[] { 0, 0, 0, 0, 0 }
            };*/
        }
        /*public DFS(int N,int DAYS)
        {
            //シフト表
            schedule = new int[N][];
            for(int i = 0; i < schedule.Length; i++)
            {
                schedule[i] = new int[DAYS];
            }
        }*/

        public void Search(uint key,int box)
        {
            Open(key, box,0);
        }

        private void Open(uint key, int box, int day)
        {
            //シフト表の外
            if (schedule[0].Length <= day)
            {
                Console.WriteLine("LastDay!!!");

                return;
            }

            //開店可能
            if (CanOpenClose(key, OPEN, day))
            {
                Console.WriteLine("Day" + day + " Open!");

                //業務開始
                DepositKey(key, box, day, OPEN);
            }
        }

        private void DepositKey(uint key, int box, int day, int time)
        {
            //全員についてチェック
            for (int p = 0; p < schedule.Length; p++)
            {
                //勤務時間
                if (schedule[p][day] == time)
                {
                    //鍵を所持
                    if ((key >> p & ONE) != 0)
                    {
                        //鍵を手放す
                        key = key ^ (ONE << p);
                        //鍵を預ける
                        box++;
                    }
                }
            }

            //鍵回収フェイズへ
            GetKey(key, box, day, time, 0);
        }

        private void GetKey(uint key, int box, int day, int time, int p_start)
        {
            //行動を未決定の従業員についてチェック
            for (int p = p_start; p < schedule.Length; p++)
            {
                //勤務時間
                if (schedule[p][day] == time)
                {
                    //預けられている鍵がある
                    if (box >= 1)
                    {
                        //鍵を取る
                        GetKey(key | (ONE << p), box - 1, day, time, p + 1);
                    }
                }
            }

            //閉店時間
            if (time >= CLOSE)
            {
                //閉店作業
                Close(key, box, day);
            }
            else
            {
                //時間を進める
                DepositKey(key, box, day, time + 1);
            }
        }

        private void Close(uint key,int box,int day)
        {
            //閉店可能
            if (CanOpenClose(key, CLOSE, day))
            {
                Open(key, box, day + 1);
            }

            return;
        }

        private bool CanOpenClose(uint key, int time, int day)
        {
            //各従業員についてチェック
            for (int p = 0; p < schedule.Length; p++)
            {
                //鍵を持っている
                if ((key >> p & ONE) != 0)
                {
                    //指定の動作が可能
                    if (schedule[p][day] == time)
                    {
                        return true;
                    }
                }
            }

            //開け閉め不可
            return false;
        }
    }
}
