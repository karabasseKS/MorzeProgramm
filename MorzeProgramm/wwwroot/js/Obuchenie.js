const morseMap = { /* …массив символов, как раньше… */ };
const morzeGroups = [ /* …массив блоков… */];

let currentBlock = [];
let currentIndex = 0;
let currentSymbol = '';
let audioCtx = new (window.AudioContext || window.webkitAudioContext)();

// Функции блока обучения
function selectBlock(idx) {
    currentBlock = morzeGroups[idx];
    currentIndex = 0;
    playCurrentSymbol();
}
function playCurrentSymbol() {
    currentSymbol = currentBlock[currentIndex];
    playMorse(currentSymbol);
}
function nextSymbol() {
    if (currentIndex < currentBlock.length - 1) {
        currentIndex++;
        playCurrentSymbol();
    }
}
function prevSymbol() {
    if (currentIndex > 0) {
        currentIndex--;
        playCurrentSymbol();
    }
}

// Генерация звука Морзе
function playMorse(symbol) {
    const code = morseMap[symbol];
    if (!code) return;
    let time = audioCtx.currentTime;
    for (let ch of code) {
        const dur = (ch === '.') ? 0.1 : 0.3;
        const osc = audioCtx.createOscillator();
        const gain = audioCtx.createGain();
        osc.frequency.setValueAtTime(700, time);
        gain.gain.setValueAtTime(0.5, time);
        osc.connect(gain).connect(audioCtx.destination);
        osc.start(time);
        osc.stop(time + dur);
        time += dur + 0.1;
    }
}

// 🧪 Квиз с вводом клавиш
let quizGroups = [];
let quizIdx = 0;
let correctQuiz = 0;

function startQuiz() {
    quizGroups = Array.from({ length: 10 }, () =>
        Array.from({ length: currentBlock.length === 4 ? 4 : 4 }, () =>
            currentBlock[Math.floor(Math.random() * currentBlock.length)]
        )
    );
    quizIdx = 0;
    correctQuiz = 0;
    document.getElementById('quizCounter').innerText = '1';
    document.getElementById('quizFeedback').innerText = '';
    playQuizGroup();
    document.addEventListener('keydown', onQuizKey);
}

function playQuizGroup() {
    const group = quizGroups[quizIdx];
    let time = audioCtx.currentTime;
    for (let sym of group) {
        for (let ch of morseMap[sym]) {
            const dur = ch === '.' ? 0.1 : 0.3;
            const osc = audioCtx.createOscillator();
            const gain = audioCtx.createGain();
            osc.frequency.setValueAtTime(700, time);
            gain.gain.setValueAtTime(0.5, time);
            osc.connect(gain).connect(audioCtx.destination);
            osc.start(time);
            osc.stop(time + dur);
            time += dur + 0.1;
        }
        time += 0.4; // пауза между символами
    }
}

let quizInput = [];

function onQuizKey(e) {
    const key = e.key.toUpperCase();
    if (!morseMap[key]) return;
    quizInput.push(key);
    if (quizInput.length === quizGroups[quizIdx].length) {
        checkQuizAnswer();
    }
}

function checkQuizAnswer() {
    const inputStr = quizInput.join('');
    const correctStr = quizGroups[quizIdx].join('');

    if (inputStr === correctStr) {
        correctQuiz++;
        document.getElementById('quizFeedback').innerText = '✅ Верно!';
    } else {
        document.getElementById('quizFeedback').innerText = `❌ Было: ${correctStr}`;
    }

    quizIdx++;
    document.getElementById('quizCounter').innerText = (quizIdx + 1) <= 10 ? (quizIdx + 1) : '✔';

    quizInput = [];

    if (quizIdx < 10) {
        setTimeout(playQuizGroup, 1000);
    } else {
        document.removeEventListener('keydown', onQuizKey);
        document.getElementById('quizFeedback').innerText += `\nПравильных: ${correctQuiz}/10`;
    }
}
