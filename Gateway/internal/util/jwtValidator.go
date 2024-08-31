package util

import (
	"fmt"
	"os"

	"github.com/golang-jwt/jwt/v5"
)

func JWTValidator(tokenstring string) error {
	key, err := os.ReadFile("jwt.key.pub")
	if err != nil {
		return err
	}
	pubKey, err := jwt.ParseRSAPublicKeyFromPEM(key)
	if err != nil {
		return err
	}

	token, parseError := jwt.Parse(tokenstring, func(t *jwt.Token) (interface{}, error) {
		return pubKey, nil
	})
	if parseError != nil {
		return parseError
	}
	if !token.Valid {
		return fmt.Errorf("invalid token")
	}

	return nil
}
