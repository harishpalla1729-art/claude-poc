AI Agent (Minimal)

Overview
- Provides a minimal, extensible AI agent that calls an OpenAI-compatible API.
- Interactive REPL with simple file-backed memory.

Quickstart
1. Create a Python virtualenv and install dependencies:

```bash
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
```

2. Set your OpenAI API key:

```bash
export OPENAI_API_KEY="sk-..."
```

3. Run the agent:

```bash
python run_agent.py --model gpt-4
```

Files created
- `agent/agent.py`: Core agent implementation.
- `run_agent.py`: CLI entrypoint.
- `requirements.txt`: Python dependencies.
- `.gitignore`: Ignore rules.
- `README_AGENT.md`: This quickstart.

Notes
- The agent persists conversation lines to `agent_memory.txt` by default.
- To change the memory file, set environment variable `AGENT_MEMORY_FILE`.
- The agent is intentionally small and modular; you can extend it with tool integrations, richer memory, or a web UI.
