import os
from typing import Optional
import openai

class AIAgent:
    """Minimal, extensible AI agent that talks to an OpenAI-compatible API.

    Features:
    - Reads `OPENAI_API_KEY` from env (or accepts it at init).
    - Simple file-backed memory (`agent_memory.txt`).
    - Interactive REPL loop via `run()`.
    """

    def __init__(self, api_key: Optional[str] = None, model: str = "gpt-4"):
        self.api_key = api_key or os.getenv("OPENAI_API_KEY")
        if not self.api_key:
            raise RuntimeError("OPENAI_API_KEY not set; export it or pass api_key to AIAgent")
        openai.api_key = self.api_key
        self.model = model
        self.memory_file = os.getenv("AGENT_MEMORY_FILE", "agent_memory.txt")

    def call_llm(self, prompt: str, temperature: float = 0.7) -> str:
        resp = openai.ChatCompletion.create(
            model=self.model,
            messages=[{"role": "user", "content": prompt}],
            temperature=temperature,
            max_tokens=500,
        )
        return resp.choices[0].message.content.strip()

    def append_memory(self, text: str) -> None:
        with open(self.memory_file, "a", encoding="utf-8") as f:
            f.write(text.strip() + "\n")

    def read_memory(self, n_lines: int = 20) -> str:
        if not os.path.exists(self.memory_file):
            return ""
        with open(self.memory_file, "r", encoding="utf-8") as f:
            lines = f.read().splitlines()
        return "\n".join(lines[-n_lines:])

    def build_prompt(self, user_input: str) -> str:
        mem = self.read_memory()
        prompt_parts = ["You are a helpful assistant."]
        if mem:
            prompt_parts.append("Recent memory:\n" + mem)
        prompt_parts.append("User:\n" + user_input)
        return "\n\n".join(prompt_parts)

    def run(self) -> None:
        print("AI Agent started. Type 'exit' or 'quit' to stop.")
        try:
            while True:
                user = input("You: ")
                if user.strip().lower() in ("exit", "quit"):
                    print("Goodbye.")
                    break
                prompt = self.build_prompt(user)
                reply = self.call_llm(prompt)
                print("Agent:", reply)
                self.append_memory(f"User: {user}")
                self.append_memory(f"Agent: {reply}")
        except KeyboardInterrupt:
            print("\nInterrupted. Exiting.")
