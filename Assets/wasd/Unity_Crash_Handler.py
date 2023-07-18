import PySimpleGUI as sg
import sys, string, os

font = ("Impact, 16")
sg.theme('LightGrey1')
layout = [
    [sg.Text("Application Pleh! encountered an error and had to force close.", font=font)],
    [sg.Text("Error message:", font=font)],
    [sg.Text("collisionHandler.cs 46, 8: Type triggger2D is not a defined type", font=font)],
    [sg.Text("Please restart the game after sending report message", font=font)],
    [sg.Button("Send report crash error")],
    [sg.Button("Do not send report crash error")]
]

window = sg.Window("Unity Crash Handler", layout, margins=(45, 45))

while True:
    event, values, = window.read()

    if event == "Send report crash error" or event == "Do not send report crash error" or event == sg.WINDOW_CLOSED:
        break

path = os.getcwd()
print(f"{path}..\ALNE.exe")
os.startfile(r"{path}ALNE.exe")