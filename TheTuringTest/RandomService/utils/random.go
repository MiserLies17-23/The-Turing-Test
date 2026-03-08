package utils

import "math/rand"

func GenerateNumberForModel() int {
	number := rand.Intn(2)
	return number
}
