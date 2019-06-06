using System;
using System.Collections.Generic;

/// <summary>
/// Neural Network C#
/// </summary>
public class NeuralNetwork : IComparable<NeuralNetwork>
{
	private int[] _katmanlar;
	public int[] Katmanlar { get { return _katmanlar; } }

	private float[][] _noronlar;
	public float[][] Noronlar { get { return _noronlar; } }

	private float[][][] _agirliklar;
	public float[][][] Agirliklar { get { return _agirliklar; } }

	private float _basari = 0f; //fitness of the network
	public float Basari { get { return _basari; } set { _basari = value; } }

	private float _mutasyonGucu = 2f;
	public float MutasyonGucu { get { return _mutasyonGucu; } set { _mutasyonGucu = Math.Abs(value); } }

	public NeuralNetwork(int[] katmanlar)
	{
		this._katmanlar = new int[katmanlar.Length];
		for (int i = 0; i < katmanlar.Length; i++)
			this._katmanlar[i] = katmanlar[i];

		NORONLARı_AYARLA();
		AGıRLıKLARı_AYARLA();
	}

	public NeuralNetwork(NeuralNetwork agKopyasi)
	{
		this._katmanlar = new int[agKopyasi._katmanlar.Length];
		for (int i = 0; i < agKopyasi._katmanlar.Length; i++)
			this._katmanlar[i] = agKopyasi._katmanlar[i];

		NORONLARı_AYARLA();
		AGıRLıKLARı_AYARLA();
		AGıRLıKLARı_KOPYALA(agKopyasi._agirliklar);
	}

	private void AGıRLıKLARı_KOPYALA(float[][][] agirliklarinKopyasi)
	{
		for (int i = 0; i < _agirliklar.Length; i++)
			for (int j = 0; j < _agirliklar[i].Length; j++)
				for (int k = 0; k < _agirliklar[i][j].Length; k++)
					_agirliklar[i][j][k] = agirliklarinKopyasi[i][j][k];
	}

	private void NORONLARı_AYARLA()
	{
		List<float[]> noronlarinListesi = new List<float[]>(); // Nöron hazırlığı.
		for (int i = 0; i < _katmanlar.Length; i++) // Tüm katmanlar üzerinde çalış.
			noronlarinListesi.Add(new float[_katmanlar[i]]); // Nöron listesine katman ekle.
		_noronlar = noronlarinListesi.ToArray(); // Listeyi dizine çevir.
	}

	private void AGıRLıKLARı_AYARLA()
	{
		List<float[][]> agirliklarinListesi = new List<float[][]>(); // Daha sonra 3B dizine ile değiştirilicek olan dizin.
		for (int i = 1; i < _katmanlar.Length; i++) { // Ağırlık bağlantılı tüm nöronların yinelemesi.
			List<float[]> katmandakiAgirliklarinListesi = new List<float[]> (); // Bu geçerli katman için ayer ağırlık listesi (2B dizisine dönüştürülecektir).
			int oncekiKatmandakiNoronSayisi = _katmanlar [i - 1];
			for (int j = 0; j < _noronlar [i].Length; j++) { // Bu geçerli katmandaki tüm nöronları yinele.
				float[] noronAgirliklari = new float[oncekiKatmandakiNoronSayisi]; // Nöron ağırlıkları.

				// 1 ve -1 arasında rasgele bir ağırlık olarak ayarla.
				for (int k = 0; k < oncekiKatmandakiNoronSayisi; k++)
					noronAgirliklari [k] = RASGELE_DEGER (-1f, 1f);
				katmandakiAgirliklarinListesi.Add (noronAgirliklari);
			}
			agirliklarinListesi.Add (katmandakiAgirliklarinListesi.ToArray ());
		}
		_agirliklar = agirliklarinListesi.ToArray();
	}

	public float[] ıLERı_HESAPLAMA(float[] girdiler)
	{
		for (int i = 0; i < girdiler.Length; i++)        
			_noronlar[0][i] = girdiler[i];
		
		// i - Katmanlar
		// j - Nöronlar
		// k - Ağırlıklar
		for (int i = 1; i < _katmanlar.Length; i++)
			for (int j = 0; j < _noronlar[i].Length; j++) {
				float net = 0f;
				for (int k = 0; k < _noronlar[i-1].Length; k++)
					net += _agirliklar[i - 1][j][k] * _noronlar[i - 1][k];
				_noronlar[i][j] = (float)Math.Tanh(net);
			}

		return _noronlar[_noronlar.Length - 1];
	}
		
	public void MUTASYON()
	{
		for (int i = 0; i < _agirliklar.Length; i++)
			for (int j = 0; j < _agirliklar [i].Length; j++)
				for (int k = 0; k < _agirliklar [i] [j].Length; k++) {
					float agirlik = _agirliklar [i] [j] [k];

					float randomNumber = RASGELE_DEGER(0f,100f);
					if (randomNumber <= 2f) {
						//flip sign of weight
						agirlik *= -1f;
					}
					else if (randomNumber <= 4f) { 
						//pick random weight between -1 and 1
						agirlik = RASGELE_DEGER(-0.5f, 0.5f);
					}
					else if (randomNumber <= 6f) { 
						//randomly increase by 0% to 100%
						float faktor = RASGELE_DEGER(0f, 1f) + 1f;
						agirlik *= faktor;
					}
					else if (randomNumber <= 8f) { 
						//randomly decrease by 0% to 100%
						float faktor = RASGELE_DEGER(0f, 1f);
						agirlik *= faktor;
					}

					_agirliklar [i] [j] [k] = agirlik;
				}
	}

	Random random = new Random(DateTime.Now.Millisecond);
	private float RASGELE_DEGER(double minimum, double maximum)
	{
		return (float)(random.NextDouble() * (maximum - minimum) + minimum);
	}

	private float sigmoid(float value){
		return (float)(1f / (1f + Math.Pow(Math.E, -value)));
	}

	public int CompareTo(NeuralNetwork diger)
	{
		if (diger == null) return 1;

		if (Basari > diger.Basari)
			return 1;
		else if (Basari < diger.Basari)
			return -1;
		else
			return 0;
	}
}