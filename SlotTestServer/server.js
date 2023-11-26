require("readline").emitKeypressEvents(process.stdin);
process.stdin.setRawMode(true);

const WebSocket = require("ws");
const PORT = 3333;
const server = new WebSocket.Server({ port: PORT });

const buttons = {
  left: {
    button: "play_8",
    active: false,
  },
  right: {
    button: "play_38",
    active: false,
  },
  up: {
    button: "bet_2",
    active: false,
  },
  down: {
    button: "play_18",
    active: false,
  },
  shoot: {
    button: "play",
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
      socket.send(JSON.stringify(payload));
    }

    if (char === "p") process.exit();
  });
});

console.log("WebSocket server is running on port " + PORT);
