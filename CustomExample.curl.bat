@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo Custom Testing Example for MTCG
echo.

show off sacrifice mechanic
curl -X POST http://localhost:10001/sacrifice --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "{\"Sacrifice\":\"<cardid>\", \"Reciever\":\"<cardid>\"}"

@echo on