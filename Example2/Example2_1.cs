// В целом, вижу много точек улучшения кода
// - Есть проблема с модификацией состояния через healthView. Предпочитаю MVVM подход с разделением ответственности.
//   В рамках данного примера, я думаю, было бы лучше иметь IHealthViewModel, который бы модифицировал поле Color.
//   TextView в свою очередь через реактивное свойство получало изменение цвета. Само view спавнилось через отдельный
//   UIService, а мы бы получили только ViewModel.
//	 IHealthViewModel также, в идеале, должен принимать HealthModel, который содержит дефолтное значение отображаемого
//   здоровья
// - В целом, максимально стараюсь избегать паттерна Observer. На мой взгляд, он создает много проблем, такие как:
//   - Сложность отладки
//   - Риски связанные с неотпиской
//	 - Риски связанные с инвалидностью подписчика

public class ExtPlayer : Player
{
	public delegate void HealthChangedDelegate(int oldHealth, int newHealth);

	public event HealthChangedDelegate HealthChanged;
}

class ExtProgram : Program
{
	// Виджет, отображающий игроку здоровье.
	private static TextView healthView = new TextView();

	// Из-за статической природы метода нарушается полиформизм
	public static void ExtMain(string[] args)
	{
		// Вызов кода по созданию игрока.
		// Если это метод только по созданию игрока, то должен быть более конкретный нейминг
		Main(args);

		healthView.Text = player.Health.ToString();

		// В примере отсутствует отписка от делегата и нет как такого workaround по dispose объекта.
		// Как вариант можно решения, можно предложить классу реализовать интерфейс IDisposable, в Dispose добавить
		// отписку.
		// Но тут также потребуется контроллер ресурсов, который бы диспозил объект при необходимости
		player.HealthChanged += OnPlayerHealthChanged;

		// Ударяем игрока.
		HitPlayer();
	}

	private static void OnPlayerHealthChanged(int oldHealth, int newHealth)
	{
		healthView.Text = newHealth.ToString();

		// Magic number. Желательно вынести в константу с названием описывающим природу значения
		//
		// Условие, при котором, меняется состояние View выглядит неясным.
		// Если мы хотим, чтобы цвет View становился красным ниже определенного порога, то условие выглядить примерно так:
		//		newHealth <= lowHPThreshold
		// Если мы хотим, подсвечивать View красным в случае нанесения большого урона, тогда так:
		//		oldHealth - newHealth >= bigDamageThreshold
		// В этом случае, также нужна система, которая бы меняла цвет обратно в дефолтный
		if (newHealth - oldHealth < -10) {
			healthView.Color = Color.Red;
		} else {
			healthView.Color = Color.White;
		}
	}
}
