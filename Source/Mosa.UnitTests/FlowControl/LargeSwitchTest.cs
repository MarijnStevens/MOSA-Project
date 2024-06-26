﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.FlowControl;

public class LargeSwitchTest
{
	#region LargeSwitchFixture test

	[MosaUnitTest(Series = "I4")]
	public static int LargeSwitchTest1(int a)
	{
		return a switch
		{
			1 => 3,
			2 => 6,
			3 => 9,
			4 => 12,
			5 => 15,
			6 => 18,
			7 => 21,
			8 => 24,
			9 => 27,
			10 => 30,
			11 => 33,
			12 => 36,
			13 => 39,
			14 => 42,
			15 => 45,
			16 => 48,
			17 => 51,
			18 => 54,
			19 => 57,
			20 => 60,
			21 => 63,
			22 => 66,
			23 => 69,
			24 => 72,
			25 => 75,
			26 => 78,
			27 => 81,
			28 => 84,
			29 => 87,
			30 => 90,
			31 => 93,
			32 => 96,
			33 => 99,
			34 => 102,
			35 => 105,
			36 => 108,
			37 => 111,
			38 => 114,
			39 => 117,
			40 => 120,
			41 => 123,
			42 => 126,
			43 => 129,
			44 => 132,
			45 => 135,
			46 => 138,
			47 => 141,
			48 => 144,
			49 => 147,
			50 => 150,
			51 => 153,
			52 => 156,
			53 => 159,
			54 => 162,
			55 => 165,
			56 => 168,
			57 => 171,
			58 => 174,
			59 => 177,
			60 => 180,
			61 => 183,
			62 => 186,
			63 => 189,
			64 => 192,
			65 => 195,
			66 => 198,
			67 => 201,
			68 => 204,
			69 => 207,
			70 => 210,
			71 => 213,
			72 => 216,
			73 => 219,
			74 => 222,
			75 => 225,
			76 => 228,
			77 => 231,
			78 => 234,
			79 => 237,
			80 => 240,
			81 => 243,
			82 => 246,
			83 => 249,
			84 => 252,
			85 => 255,
			86 => 258,
			87 => 261,
			88 => 264,
			89 => 267,
			90 => 270,
			91 => 273,
			92 => 276,
			93 => 279,
			94 => 282,
			95 => 285,
			96 => 288,
			97 => 291,
			98 => 294,
			99 => 297,
			100 => 300,
			101 => 303,
			102 => 306,
			103 => 309,
			104 => 312,
			105 => 315,
			106 => 318,
			107 => 321,
			108 => 324,
			109 => 327,
			110 => 330,
			111 => 333,
			112 => 336,
			113 => 339,
			114 => 342,
			115 => 345,
			116 => 348,
			117 => 351,
			118 => 354,
			119 => 357,
			120 => 360,
			121 => 363,
			122 => 366,
			123 => 369,
			124 => 372,
			125 => 375,
			126 => 378,
			127 => 381,
			128 => 384,
			129 => 387,
			130 => 390,
			131 => 393,
			132 => 396,
			133 => 399,
			134 => 402,
			135 => 405,
			136 => 408,
			137 => 411,
			138 => 414,
			139 => 417,
			140 => 420,
			141 => 423,
			142 => 426,
			143 => 429,
			144 => 432,
			145 => 435,
			146 => 438,
			147 => 441,
			148 => 444,
			149 => 447,
			150 => 450,
			151 => 453,
			152 => 456,
			153 => 459,
			154 => 462,
			155 => 465,
			156 => 468,
			157 => 471,
			158 => 474,
			159 => 477,
			160 => 480,
			161 => 483,
			162 => 486,
			163 => 489,
			164 => 492,
			165 => 495,
			166 => 498,
			167 => 501,
			168 => 504,
			169 => 507,
			170 => 510,
			171 => 513,
			172 => 516,
			173 => 519,
			174 => 522,
			175 => 525,
			176 => 528,
			177 => 531,
			178 => 534,
			179 => 537,
			180 => 540,
			181 => 543,
			182 => 546,
			183 => 549,
			184 => 552,
			185 => 555,
			186 => 558,
			187 => 561,
			188 => 564,
			189 => 567,
			190 => 570,
			191 => 573,
			192 => 576,
			193 => 579,
			194 => 582,
			195 => 585,
			196 => 588,
			197 => 591,
			198 => 594,
			199 => 597,
			200 => 600,
			201 => 603,
			202 => 606,
			203 => 609,
			204 => 612,
			205 => 615,
			206 => 618,
			207 => 621,
			208 => 624,
			209 => 627,
			210 => 630,
			211 => 633,
			212 => 636,
			213 => 639,
			214 => 642,
			215 => 645,
			216 => 648,
			217 => 651,
			218 => 654,
			219 => 657,
			220 => 660,
			221 => 663,
			222 => 666,
			223 => 669,
			224 => 672,
			225 => 675,
			226 => 678,
			227 => 681,
			228 => 684,
			229 => 687,
			230 => 690,
			231 => 693,
			232 => 696,
			233 => 699,
			234 => 702,
			235 => 705,
			236 => 708,
			237 => 711,
			238 => 714,
			239 => 717,
			240 => 720,
			241 => 723,
			242 => 726,
			243 => 729,
			244 => 732,
			245 => 735,
			246 => 738,
			247 => 741,
			248 => 744,
			249 => 747,
			250 => 750,
			251 => 753,
			252 => 756,
			253 => 759,
			254 => 762,
			255 => 765,
			256 => 768,
			257 => 771,
			258 => 774,
			259 => 777,
			260 => 780,
			261 => 783,
			262 => 786,
			263 => 789,
			264 => 792,
			265 => 795,
			266 => 798,
			267 => 801,
			268 => 804,
			269 => 807,
			270 => 810,
			271 => 813,
			272 => 816,
			273 => 819,
			274 => 822,
			275 => 825,
			276 => 828,
			277 => 831,
			278 => 834,
			279 => 837,
			280 => 840,
			281 => 843,
			282 => 846,
			283 => 849,
			284 => 852,
			285 => 855,
			286 => 858,
			287 => 861,
			288 => 864,
			289 => 867,
			290 => 870,
			291 => 873,
			292 => 876,
			293 => 879,
			294 => 882,
			295 => 885,
			296 => 888,
			297 => 891,
			298 => 894,
			299 => 897,
			300 => 900,
			301 => 903,
			302 => 906,
			303 => 909,
			304 => 912,
			305 => 915,
			306 => 918,
			307 => 921,
			308 => 924,
			309 => 927,
			310 => 930,
			311 => 933,
			312 => 936,
			313 => 939,
			314 => 942,
			315 => 945,
			316 => 948,
			317 => 951,
			318 => 954,
			319 => 957,
			320 => 960,
			321 => 963,
			322 => 966,
			323 => 969,
			324 => 972,
			325 => 975,
			326 => 978,
			327 => 981,
			328 => 984,
			329 => 987,
			330 => 990,
			331 => 993,
			332 => 996,
			333 => 999,
			334 => 1002,
			335 => 1005,
			336 => 1008,
			337 => 1011,
			338 => 1014,
			339 => 1017,
			340 => 1020,
			341 => 1023,
			342 => 1026,
			343 => 1029,
			344 => 1032,
			345 => 1035,
			346 => 1038,
			347 => 1041,
			_ => a
		};
	}

	#endregion LargeSwitchFixture test
}
