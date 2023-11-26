# SlotTestServer

## TLDR

This server will be available in port `3333`. When a client (only one please, I doubt it will work with more :) ) connects to it, commands can be sent to it, simulating interaction with the quixant's buttons (with the ones relevant to our game).

Due to limitations with node's IO input, these actions are performed by toggling:

- the first press of a key simulates "holding down" the corresponding quixant's button

- the second press of a key simulates "releasing" the corresponding quixant's button

The payload sent to the Unity client is the following:

```
{
    id: "service::hardware/inputs/status_change",
    payload: {
      data: {
        ascii_code: 32,
        input_address: 6,
        input_state: state,
        name: button,
        output_state: "inactive",
        type: "input_output",
      },
      status_code: 200,
    },
}
```

## Install

Install dependencies with `npm install` on this directory

## Usage

Run `node server.json` in this directory.

The program will hijack stdin, thus preventing any signals from being sent as well. The following actions will be performed depending on which keys are pressed (inside the terminal window the app is being run in):

- a - toggles the "left" button (equivalent to the quixant's "play_8" button)

- d - toggles the "right" button (equivalent to the quixant's "play_38" button)

- s - toggles the "down" button (equivalent to the quixant's "bet_2" button)

- w - toggles the "up" button (equivalent to the quixant's "play_18" button)

- l - toggles the "shoot" button (equivalent to the quixant's "play" button)

- p - closes the server (not used in our game)
