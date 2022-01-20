@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo Custom Testing Example for MTCG
echo.

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"manuel\", \"Password\":\"eiwen\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"test\", \"Password\":\"test\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\", \"Password\":\"istrator\"}"
echo.
echo should fail:
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"manuel\", \"Password\":\"eiwen\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"test\", \"Password\":\"test\"}"
echo.

REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"manuel\", \"Password\":\"eiwen\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"test\", \"Password\":\"test\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\", \"Password\":\"istrator\"}"
echo.
echo should fail:
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"manuel\", \"Password\":\"test\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"test\", \"Password\":\"eiwen\"}"
echo.

REM --------------------------------------------------
echo 3) create packages (done by "admin")
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"Name\":\"WaterGoblin\", \"Damage\": 10.0}, {\"Id\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"Name\":\"Dragon\", \"Damage\": 50.0}, {\"Id\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"Name\":\"WaterSpell\", \"Damage\": 20.0}, {\"Id\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Name\":\"Ork\", \"Damage\": 45.0}, {\"Id\":\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\", \"Name\":\"FireSpell\",    \"Damage\": 25.0}]"
echo.																																																																																		 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"644808c2-f87a-4600-b313-122b02322fd5\", \"Name\":\"WaterGoblin\", \"Damage\":  9.0}, {\"Id\":\"4a2757d6-b1c3-47ac-b9a3-91deab093531\", \"Name\":\"Dragon\", \"Damage\": 55.0}, {\"Id\":\"91a6471b-1426-43f6-ad65-6fc473e16f9f\", \"Name\":\"WaterSpell\", \"Damage\": 21.0}, {\"Id\":\"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\", \"Name\":\"Ork\", \"Damage\": 55.0}, {\"Id\":\"f8043c23-1534-4487-b66b-238e0c3c39b5\", \"Name\":\"WaterSpell\",   \"Damage\": 23.0}]"
echo.																																																																																		 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"b017ee50-1c14-44e2-bfd6-2c0c5653a37c\", \"Name\":\"WaterGoblin\", \"Damage\": 11.0}, {\"Id\":\"d04b736a-e874-4137-b191-638e0ff3b4e7\", \"Name\":\"Dragon\", \"Damage\": 70.0}, {\"Id\":\"88221cfe-1f84-41b9-8152-8e36c6a354de\", \"Name\":\"WaterSpell\", \"Damage\": 22.0}, {\"Id\":\"1d3f175b-c067-4359-989d-96562bfa382c\", \"Name\":\"Ork\", \"Damage\": 40.0}, {\"Id\":\"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\", \"Name\":\"RegularSpell\", \"Damage\": 28.0}]"
echo.																																																																																		 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"ed1dc1bc-f0aa-4a0c-8d43-1402189b33c8\", \"Name\":\"WaterGoblin\", \"Damage\": 10.0}, {\"Id\":\"65ff5f23-1e70-4b79-b3bd-f6eb679dd3b5\", \"Name\":\"Dragon\", \"Damage\": 50.0}, {\"Id\":\"55ef46c4-016c-4168-bc43-6b9b1e86414f\", \"Name\":\"WaterSpell\", \"Damage\": 20.0}, {\"Id\":\"f3fad0f2-a1af-45df-b80d-2e48825773d9\", \"Name\":\"Ork\", \"Damage\": 45.0}, {\"Id\":\"8c20639d-6400-4534-bd0f-ae563f11f57a\", \"Name\":\"WaterSpell\",   \"Damage\": 25.0}]"
echo.
echo should fail:
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"a\", \"Name\":\"WaterGoblin\", \"Damage\": 11.0}, {\"Id\":\"s\", \"Name\":\"Dragon\", \"Damage\": 70.0}, {\"Id\":\"d\", \"Name\":\"WaterSpell\", \"Damage\": 22.0}, {\"Id\":\"f\", \"Name\":\"RegularSpell\", \"Damage\": 28.0}]"
echo.																																																																																		 				    
curl -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[]"
echo.

REM --------------------------------------------------
echo 4) acquire packages
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic test-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic test-mtcgToken" -d ""
echo.


REM --------------------------------------------------
echo 5) configure decks
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Basic manuel-mtcgToken"
echo.
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Basic test-mtcgToken" -d "[\"b017ee50-1c14-44e2-bfd6-2c0c5653a37c\", \"d04b736a-e874-4137-b191-638e0ff3b4e7\", \"55ef46c4-016c-4168-bc43-6b9b1e86414f\", \"f3fad0f2-a1af-45df-b80d-2e48825773d9\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Basic test-mtcgToken"
echo.


REM --------------------------------------------------
echo 6) battle
start /b "manuel battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic manuel-mtcgToken"
start /b "test battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic test-mtcgToken"
ping localhost -n 10 >NUL 2>NUL

REM --------------------------------------------------
echo 7) sacrifice to make other card stronger
echo should fail:
curl -X POST http://localhost:10001/sacrifice --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d "{\"Sacrifice\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"Receiver\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\"}"
echo.
curl -X POST http://localhost:10001/sacrifice --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d "{\"Sacrifice\":\"644808c2-f87a-4600-b313-122b02322fd5\", \"Receiver\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\"}"
echo.
echo should work:
curl -X POST http://localhost:10001/sacrifice --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d "{\"Sacrifice\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Receiver\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\"}"
echo.
curl -X POST http://localhost:10001/sacrifice --header "Content-Type: application/json" --header "Authorization: Basic manuel-mtcgToken" -d "{\"Sacrifice\":\"4a2757d6-b1c3-47ac-b9a3-91deab093531\", \"Receiver\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\"}"
echo.

REM --------------------------------------------------
echo 8) show deck with updated (stronger) cards
curl -X GET http://localhost:10001/deck --header "Authorization: Basic manuel-mtcgToken"
echo.

REM --------------------------------------------------
echo 9) show all cards
curl -X GET http://localhost:10001/cards --header "Authorization: Basic manuel-mtcgToken"
echo.

@echo on