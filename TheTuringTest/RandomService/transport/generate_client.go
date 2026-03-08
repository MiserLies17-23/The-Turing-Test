package transport

import (
	"RandomService/utils"
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"net/http"
)

func PostModel() {
	data := utils.CreateModel()

	jsonData, _ := json.Marshal(data)
	fmt.Println(string(jsonData))

	go func() {
		response, err := http.Post("#",
			"application/json", bytes.NewBuffer(jsonData))

		if err != nil {
			panic(err)
		}
		defer func(Body io.ReadCloser) {
			err := Body.Close()
			if err != nil {
				panic(err)
			}
		}(response.Body)
	}()
}
