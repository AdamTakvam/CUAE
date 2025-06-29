using System;

using Metreos.Toolset.TenarySearchTree;

namespace Metreos.Toolset
{
	/// <summary>
	/// Summary description for txt2emo.
	/// </summary>
	public class txt2emo
	{
		private TstDictionary tst;
		private string traceWord = "";
		private string word = "";
		private int emoId = -2;			// -2: no match, -1: possible match, 0+: matched			

		public string Word { get { return word; } }
		public int EmoId { get { return emoId; } }

		public txt2emo()
		{
			tst = new TstDictionary();
			MakeMSNDictionary();
		}

		protected void MakeMSNDictionary()
		{
			tst.Add(":-)", 0);
			tst.Add(":)", 0);

			tst.Add(":-D", 1);
			tst.Add(":D", 1);
			tst.Add(":>", 1);
			tst.Add(":-d", 1);

			tst.Add(";-)", 2);
			tst.Add(";)", 2);

			tst.Add(":-O", 3);
			tst.Add(":o", 3);

			tst.Add(":-P", 4);
			tst.Add(":p", 4);

			tst.Add("(H)", 5);
			tst.Add("(h)", 5);

			tst.Add(":-@", 6);
			tst.Add(":@", 6);

			tst.Add(":-S", 7);
			tst.Add(":S", 7);
			tst.Add(":-s", 7);

			tst.Add(":-$", 8);

			tst.Add(":-(", 9);
			tst.Add(":(", 9);
			tst.Add(":<", 9);

			tst.Add(":'(", 10);

			tst.Add(":-|", 11);
			tst.Add(":|", 11);

			tst.Add("(A)", 12);
			tst.Add("(a)", 12);

			tst.Add("8o|", 13);

			tst.Add("8-)", 14);

			tst.Add("+o(", 15);

			tst.Add("<:o)", 16);

			tst.Add(":-#", 19);


			tst.Add(":-*", 20);

			tst.Add("(L)", 23);
			tst.Add("(l)", 23);

			tst.Add("(U)", 24);
			tst.Add("(u)", 24);

			tst.Add("(M)", 25);
			tst.Add("(m)", 25);

			tst.Add("(@)", 26);

			tst.Add("(&)", 27);

			tst.Add("(sn)", 28);

			tst.Add("(bah)", 29);

			tst.Add("(S)", 30);

			tst.Add("(*)", 31);

			tst.Add("(#)", 32);

			tst.Add("(R)", 33);
			tst.Add("(r)", 33);

			tst.Add("({)", 34);

			tst.Add("(})", 35);

			tst.Add("(K)", 36);
			tst.Add("(k)", 36);

			tst.Add("(F)", 37);
			tst.Add("(f)", 37);

			tst.Add("(W)", 38);
			tst.Add("(w)", 38);

			tst.Add("(O)", 39);
			tst.Add("(o)", 39);

			tst.Add("(G)", 40);
			tst.Add("(g)", 40);

			tst.Add("(^)", 41);

			tst.Add("(P)", 42);
			tst.Add("(p)", 42);

			tst.Add("(I)", 43);
			tst.Add("(i)", 43);

			tst.Add("(C)", 44);
			tst.Add("(c)", 44);

			tst.Add("(T)", 45);
			tst.Add("(t)", 45);

			tst.Add("(mp)", 46);

			tst.Add("(au)", 47);

			tst.Add("(ap)", 48);

			tst.Add("(co)", 49);

			tst.Add("(mo)", 50);

			tst.Add("(~)", 51);

			tst.Add("(8)", 52);

			tst.Add("(pi)", 53);

			tst.Add("(so)", 54);

			tst.Add("(E)", 55);
			tst.Add("(e)", 55);

			tst.Add("(X)", 56);
			tst.Add("(x)", 56);

			tst.Add("(Z)", 57);
			tst.Add("(z)", 57);

			tst.Add("(ip)", 58);

			tst.Add("(um)", 59);
		}

		protected void FindEmoticons()
		{
			emoId = -2;

			TstDictionaryEntry tde = tst.Find(traceWord);

			if (tde == null)
			{
				// start over
				traceWord = "";
				word = "";
			}
			else
			{
				if (tde.IsKey)
				{
					emoId = (int)tde.Value;
					word = traceWord;
					traceWord = "";

				}
				else
				{
					if (tde.HasChildren)
					{
						// partial match, keep tracing
						emoId = -1;
					}
				}
			}
		}

		public bool Lookup(char c)
		{
			traceWord = traceWord + c;
			FindEmoticons();

			if (emoId >= 0)
				return true;

			return false;
		}
	}
}
