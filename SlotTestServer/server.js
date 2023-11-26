require("readline").emitKeypressEvents(process.stdin);
process.stdin.setRawMode(true);

const WebSocket = require("ws");
const PORT = 3333;
const server = new WebSocket.Server({ port: PORT });

LEFT_BTN = "play_8";
RIGHT_BTN = "play_38";
UP_BTN = "bet_2";
DOWN_BTN = "play_18";
PLAY_BTN = "play";

const buttons = {
  left: {
    button: LEFT_BTN,
    active: false,
  },
  right: {
    button: RIGHT_BTN,
    active: false,
  },
  up: {
    button: UP_BTN,
    active: false,
  },
  down: {
    button: DOWN_BTN,
    active: false,
  },
  shoot: {
    button: PLAY_BTN,
    active: false,
  },
};

const getPayload = (button, state) => {
  return {
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
  };
};

const sendPayload = (socket, payloadJSON) =>
  socket.send(JSON.stringify(payloadJSON));

server.on("connection", (socket) => {
  console.log("Client connected");

  // Handle messages from the client
  socket.on("message", (message) => {
    console.log(`Received message: ${message}`);

    // Send a response back to the client
    // socket.send(`Server received your message: ${message}`);
  });

  // Handle the connection close
  socket.on("close", () => {
    console.log("Client disconnected");
  });

  process.stdin.on("keypress", (char, evt) => {
    let btn;
    if (char === "a") {
      btn = buttons.left;
    } else if (char == "d") {
      btn = buttons.right;
    } else if (char == "w") {
      btn = buttons.up;
    } else if (char == "s") {
      btn = buttons.down;
    } else if (char == "l") {
      btn = buttons.shoot;
    }

    if (btn != null) {
      console.log(btn.button, "|", btn.active ? "inactive" : "active");
      const payload = getPayload(
        btn.button,
        btn.active ? "inactive" : "active"
      );
      btn.active = !btn.active;
      sendPayload(payload);
    }

    if (char === "p") process.exit();
  });

  // Test multiple equal payloads
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "active")), 3000);
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "active")), 4000);
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "active")), 5000);
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "active")), 6000);
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "inactive")), 8000);
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "inactive")), 9000);
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "inactive")), 10000);
  // setTimeout(() => sendPayload(socket, getPayload(RIGHT_BTN, "inactive")), 11000);
});

console.log("WebSocket server is running on port " + PORT);
