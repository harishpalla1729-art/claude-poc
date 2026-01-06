import argparse
from agent import AIAgent


def main():
    parser = argparse.ArgumentParser(description="Run the minimal AI agent")
    parser.add_argument("--model", help="LLM model to use", default="gpt-4")
    parser.add_argument("--api-key", help="OpenAI API key (overrides OPENAI_API_KEY env)", default=None)
    args = parser.parse_args()

    agent = AIAgent(api_key=args.api_key, model=args.model)
    agent.run()


if __name__ == "__main__":
    main()
