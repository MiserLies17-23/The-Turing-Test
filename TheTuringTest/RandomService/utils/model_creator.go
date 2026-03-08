package utils

func CreateModel() map[string]string {
	Player1Number := GenerateNumberForModel()
	Player2Number := GenerateNumberForModel()
	JudgeNumber := GenerateNumberForModel()

	var Player1Model, Player2Model, JudgeModel string
	if Player1Number == 0 {
		Player1Model = "User"
	} else {
		Player1Model = "AI"
	}

	if Player2Number == 0 {
		Player2Model = "User"
	} else {
		Player2Model = "AI"
	}

	if JudgeNumber == 0 {
		JudgeModel = "User"
	} else {
		JudgeModel = "AI"
	}

	data := map[string]string{
		"Player1": Player1Model,
		"Player2": Player2Model,
		"Judge":   JudgeModel}

	return data
}
