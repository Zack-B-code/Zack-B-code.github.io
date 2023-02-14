const canvas = document.getElementById('game');
const context = canvas.getContext('2d');
const grid = 15;
const paddleHeight = grid * 5; // 80
const maxPaddleY = canvas.height - grid - paddleHeight;

var paddleSpeed = 6;
var ballSpeed = 3;

var p1score = 0;
var p2score = 0;
var scoredisplay = "";
context.font = "50px serif";

var frameRequest;
var isGameOver = 0;

const leftPaddle = {
  // start in the middle of the game on the left side
  x: grid * 2,
  y: canvas.height / 2 - paddleHeight / 2,
  width: grid,
  height: paddleHeight,

  // paddle velocity
  dy: 0
};
const rightPaddle = {
  // start in the middle of the game on the right side
  x: canvas.width - grid * 3,
  y: canvas.height / 2 - paddleHeight / 2,
  width: grid,
  height: paddleHeight,

  // paddle velocity
  dy: 0
};
const ball = {
  // start in the middle of the game
  x: canvas.width / 2,
  y: canvas.height / 2,
  width: grid,
  height: grid,

  // keep track of when need to reset the ball position
  resetting: false,

  // ball velocity (start going to the top-right corner)
  dx: ballSpeed,
  dy: -ballSpeed
};

// check for collision between two objects using axis-aligned bounding box (AABB)
// @see https://developer.mozilla.org/en-US/docs/Games/Techniques/2D_collision_detection
function collides(obj1, obj2) {
  return obj1.x < obj2.x + obj2.width &&
         obj1.x + obj1.width > obj2.x &&
         obj1.y < obj2.y + obj2.height &&
         obj1.y + obj1.height > obj2.y;
}

// Returns if the player's score has won
function playerVictory(playerScore) {
    return (playerScore >= 7);
}

// Creates a game over screen
function gameOverScreen(playerNumber) {
  cancelAnimationFrame(frameRequest);
  isGameOver = 1;
  context.textAlign = "center";
  context.fillStyle = "rgba(0, 0, 0, 0.7)";
  context.fillRect(0, 0, canvas.width, canvas.height);
  context.fillStyle = "White";
  context.fillText("Game Over", canvas.width / 2, canvas.height / 4);
  context.fillText("Player " + playerNumber + " Won!", canvas.width / 2, canvas.height / 2);
  context.fillText("Play again", canvas.width / 2, canvas.height * (3 / 4));
  // Visual box for where clickable area is.
  context.fillStyle = "rgba(160, 160, 160, 0.4)";
  context.fillRect(canvas.width * (1/3), canvas.height * (3 / 4) - 50, 250, 75);
  // Listener for clicks
  canvas.addEventListener('click', getCursorPosition);
}

// Resets the game for play again
function resetGame() {
  canvas.removeEventListener('click', getCursorPosition);
  p1score = 0;
  p2score = 0;
  frameRequest = requestAnimationFrame(loop);
}

// game loop
function loop() {
  frameRequest = requestAnimationFrame(loop);
  context.clearRect(0,0,canvas.width,canvas.height);

  // move paddles by their velocity
  leftPaddle.y += leftPaddle.dy;
  rightPaddle.y += rightPaddle.dy;

  // prevent paddles from going through walls
  if (leftPaddle.y < grid) {
    leftPaddle.y = grid;
  }
  else if (leftPaddle.y > maxPaddleY) {
    leftPaddle.y = maxPaddleY;
  }

  if (rightPaddle.y < grid) {
    rightPaddle.y = grid;
  }
  else if (rightPaddle.y > maxPaddleY) {
    rightPaddle.y = maxPaddleY;
  }

  // draw paddles
  context.fillStyle = 'white';
  context.fillRect(leftPaddle.x, leftPaddle.y, leftPaddle.width, leftPaddle.height);
  context.fillRect(rightPaddle.x, rightPaddle.y, rightPaddle.width, rightPaddle.height);

  // move ball by its velocity
  ball.x += ball.dx;
  ball.y += ball.dy;

  // prevent ball from going through walls by changing its velocity
  if (ball.y < grid) {
    ball.y = grid;
    ball.dy *= -1;
  }
  else if (ball.y + grid > canvas.height - grid) {
    ball.y = canvas.height - grid * 2;
    ball.dy *= -1;
  }

  // reset ball if it goes past paddle (but only if we haven't already done so)
  if ( (ball.x < 0 || ball.x > canvas.width) && !ball.resetting) {
    if (ball.x < 0) {
      p1score += 1
    }
    if (ball.x > canvas.width) {
      p2score += 1
    }
    scoredisplay = p2score + " - " + p1score;
    context.fillText(scoredisplay, (canvas.width / 2 - grid / 2) - 300 ,(canvas.height / 2 - paddleHeight / 2) - 200)

    ball.resetting = true;

    // give some time for the player to recover before launching the ball again
    setTimeout(() => {
      ball.resetting = false;
      ball.x = canvas.width / 2;
      ball.y = canvas.height / 2;
    }, 400);
  }

  // check to see if ball collides with paddle. if they do change x velocity
  if (collides(ball, leftPaddle)) {
    ball.dx *= -1;

    // move ball next to the paddle otherwise the collision will happen again
    // in the next frame
    ball.x = leftPaddle.x + leftPaddle.width;
  }
  else if (collides(ball, rightPaddle)) {
    ball.dx *= -1;

    // move ball next to the paddle otherwise the collision will happen again
    // in the next frame
    ball.x = rightPaddle.x - ball.width;
  }

  // draw ball
  context.fillRect(ball.x, ball.y, ball.width, ball.height);

  // draw walls
  context.fillStyle = 'lightgrey';
  context.fillRect(0, 0, canvas.width, grid);
  context.fillRect(0, canvas.height - grid, canvas.width, canvas.height);

  // draw dotted line down the middle
  for (let i = grid; i < canvas.height - grid; i += grid * 2) {
    context.fillRect(canvas.width / 2 - grid / 2, i, grid, grid);
  }

  scoredisplay = p2score + " - " + p1score;
  context.fillText(scoredisplay, (canvas.width / 2 - grid / 2) - 300 ,(canvas.height / 2 - paddleHeight / 2) - 200)
  
  if (playerVictory(p1score)) gameOverScreen(1);
  if (playerVictory(p2score)) gameOverScreen(2);
}

// listen to keyboard events to move the paddles
document.addEventListener('keydown', function(e) {

  // up arrow key
  if (e.which === 38) {
    rightPaddle.dy = -paddleSpeed;
  }
  // down arrow key
  else if (e.which === 40) {
    rightPaddle.dy = paddleSpeed;
  }

  // w key
  if (e.which === 87) {
    leftPaddle.dy = -paddleSpeed;
  }
  // a key
  else if (e.which === 83) {
    leftPaddle.dy = paddleSpeed;
  }
});

// listen to keyboard events to stop the paddle if key is released
document.addEventListener('keyup', function(e) {
  if (e.which === 38 || e.which === 40) {
    rightPaddle.dy = 0;
  }

  if (e.which === 83 || e.which === 87) {
    leftPaddle.dy = 0;
  }
});

function getCursorPosition(event) {
  let rect = canvas.getBoundingClientRect();
  let x = event.clientX - rect.left;
  let y = event.clientY - rect.top;
  if (isPlayAgainPressed(x,y)) resetGame();
}

function isPlayAgainPressed(x, y) {
  return  ((y > (canvas.height * (3 / 4) - 50)) &&
          (y < (canvas.height * (3 / 4) - 50 + 75)) &&
          (x > (canvas.width * (1/3))) &&
          (x < (canvas.width * (1/3) + 250)));
}

// start the game
frameRequest = requestAnimationFrame(loop);
